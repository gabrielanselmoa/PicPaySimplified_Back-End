using PicPaySimplified.Application.Dtos.Payment;
using PicPaySimplified.Application.Dtos.Users;
using PicPaySimplified.Domain.Entities;

namespace PicPaySimplified.Application.Mappers;

public static class PaymentMapper
{
    public static PaymentResponse ToResponse(Payment payment, UserPreview payer, UserPreview payee)
    {
        return new PaymentResponse()
        {
            Value = payment.Amount,
            Payer = payer,
            Payee = payee,
            Currency = payment.Currency,
            Timestamp = payment.Timestamp,
            Authorization = payment.AuthStatus,
            Notification = payment.NotificationStatus,
        };
    }

    public static Payment ToDomain(PaymentRequest request)
    {
        return new Payment()
        {
            Amount = request.Value,
            PayerId = request.Payer,
            PayeeId = request.Payee,
        };
    }
}