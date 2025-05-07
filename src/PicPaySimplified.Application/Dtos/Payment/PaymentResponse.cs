using System.Text.Json.Serialization;
using PicPaySimplified.Application.Dtos.Users;
using PicPaySimplified.Domain.Entities.Enums;

namespace PicPaySimplified.Application.Dtos.Payment;

public record PaymentResponse
{
    public Decimal Value { get; init; }
    public UserPreview Payer { get; init; } = null!;
    public UserPreview Payee { get; init; } = null!;
    public string Currency { get; init; } = null!;
    public DateTime Timestamp { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AuthorizationStatus Authorization { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NotificationStatus Notification { get; init; }
}