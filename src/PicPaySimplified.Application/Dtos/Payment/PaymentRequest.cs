using System.ComponentModel.DataAnnotations;

namespace PicPaySimplified.Application.Dtos.Payment;

public record PaymentRequest(
    [Required, Range(0.01, 10000)] Decimal Value, 
    [Required] string Payer, 
    [Required] string Payee);