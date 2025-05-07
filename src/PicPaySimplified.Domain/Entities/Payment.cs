using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PicPaySimplified.Domain.Entities.Enums;

namespace PicPaySimplified.Domain.Entities;

public class Payment
{
    [BsonId] [BsonElement("id")] [BsonRepresentation(BsonType.ObjectId)] public string Id { get; set; } = null!;
    [BsonElement("payer_id")] public string PayerId { get; set; } = null!;
    [BsonElement("payee_id")] public string PayeeId { get; set; } = null!;
    [BsonElement("amount")] public decimal Amount { get; set; }
    [BsonElement("currency")] public string Currency { get; set; } = "BRL";
    [BsonElement("timestamp")] public DateTime Timestamp { get; set; } =  DateTime.Now;
    [BsonElement("status")] [BsonRepresentation(BsonType.String)] public Status Status { get; set; }
    [BsonElement("authorization_status")] [BsonRepresentation(BsonType.String)] public AuthorizationStatus AuthStatus { get; set; }
    [BsonElement("notification_status")] [BsonRepresentation(BsonType.String)] public NotificationStatus NotificationStatus { get;
        set; }
}