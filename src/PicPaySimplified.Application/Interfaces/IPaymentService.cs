using PicPaySimplified.Application.Dtos.Payment;
using PicPaySimplified.Domain.Entities;

namespace PicPaySimplified.Application.Interfaces;

public interface IPaymentService
{
    public Task<ResultModel<ICollection<PaymentResponse>>> Retrieve(string id);
    public Task<ResultModel<PaymentResponse?>> Handle(PaymentRequest request);
}