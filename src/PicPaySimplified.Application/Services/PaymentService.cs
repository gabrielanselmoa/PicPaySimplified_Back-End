using PicPaySimplified.Application.Dtos.Payment;
using PicPaySimplified.Application.Interfaces;
using PicPaySimplified.Application.Mappers;
using PicPaySimplified.Domain.Entities;
using PicPaySimplified.Domain.Entities.Enums;
using PicPaySimplified.Domain.Interfaces;

namespace PicPaySimplified.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    
    private readonly IExternalAuthorizationService _authHttp;
    private readonly IExternalNotificationService _notifyHttp;

    public PaymentService(IPaymentRepository paymentRepository, IUserRepository userRepository, IExternalAuthorizationService externalAuthorizationService, IExternalNotificationService externalNotificationService)
    {
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
        _authHttp = externalAuthorizationService;
        _notifyHttp = externalNotificationService;
    }
    
    public async Task<ResultModel<ICollection<PaymentResponse>>> Retrieve(string id)
    {
        var response = new ResultModel<ICollection<PaymentResponse>>();
        var user = await _userRepository.GetById(id);
        if (user == null)
        {
            response.Message = "User not found";
            response.Success = false;
            return response;
        }
        var payments = await _paymentRepository.GetAll(user);
        var dto = new List<PaymentResponse>();
        User payer;
        User payee;
        foreach (var payment in payments)
        {
            payer = (await _userRepository.GetById(payment.PayerId))!;
            payee = (await _userRepository.GetById(payment.PayeeId))!;
            
            var previewPayer = UserMapper.ToPreview(payer); 
            var previewPayee = UserMapper.ToPreview(payee);

            dto.Add(PaymentMapper.ToResponse(payment, previewPayer, previewPayee));
        }
        response.Data = dto;
        response.Message = "Success";
        response.Success = true;
        return response;
    }

    public async Task<ResultModel<PaymentResponse?>> Handle(PaymentRequest request)
    {
        var response = new ResultModel<PaymentResponse?>();
        var payer = await _userRepository.GetById(request.Payer);
        var payee = await _userRepository.GetById(request.Payee);
        
        if (payer == null || payee == null)
        {
            response.Message = "User(s) not found";
            response.Success = false;
            return response;
        }
        
        if (payer.Type != UserType.Normal)
        {
            response.Message = "User ShopKeeper cannot execute transactions!";
            response.Success = false;
            return response;
        }

        if (payer.Balance == 0 || payer.Balance < request.Value)
        {
            response.Message = "Payer does not have enough money!";
            response.Success = false;
            return response;
        }
        
        var paymentDomain = PaymentMapper.ToDomain(request);
        
        var isAuthorized = await _authHttp.Authorize();
        if (!isAuthorized)
        {
            paymentDomain.AuthStatus = AuthorizationStatus.DENIED;
            paymentDomain.NotificationStatus = NotificationStatus.FAILED;
            await _paymentRepository.Create(paymentDomain);
            response.Message = "Payment is not authorized!";
            response.Success = false;
            return response;
        }
        
        paymentDomain.AuthStatus = AuthorizationStatus.AUTHORIZED;
        
        var wasNotified = await _notifyHttp.Notify();
        if (!wasNotified)
            paymentDomain.NotificationStatus =  NotificationStatus.FAILED;
        
        paymentDomain.NotificationStatus = NotificationStatus.SENT;
        
        payer.Balance -= request.Value;
        await _userRepository.UpdateBalance(payer.Id, payer.Balance);
        payee.Balance += request.Value;
        await _userRepository.UpdateBalance(payee.Id, payee.Balance);
        
        var previewPayer = UserMapper.ToPreview(payer); 
        var previewPayee = UserMapper.ToPreview(payee);
        
        await _paymentRepository.Create(paymentDomain);
        response.Data = PaymentMapper.ToResponse(paymentDomain, previewPayer, previewPayee);
        response.Message = "Success";
        response.Success = true;
        return response;
    }
}