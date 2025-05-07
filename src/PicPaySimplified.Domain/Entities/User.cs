using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PicPaySimplified.Domain.Entities.Enums;

namespace PicPaySimplified.Domain.Entities;

public class User
{
    [BsonId] [BsonElement("id")] [BsonRepresentation(BsonType.ObjectId)] public string Id { get; set; } =  null!;
    [BsonElement("full_name")] public string FullName { get; set; } = null!;
    [BsonElement("email")] public string Email { get; set; } = null!;
    [BsonElement("password")] public string Password { get; set; } = null!;
    [BsonElement("cpf")] public string? Cpf { get; set; }
    [BsonElement("balance")] public Decimal Balance { get; set; }
    [BsonElement("created_at")] public DateTime CreationDate { get; set; } = DateTime.Now;
    [BsonElement("is_active")] public bool IsActive { get; set; }
    [BsonElement("user_type")] [BsonRepresentation(BsonType.String)] public UserType Type { get; set; } = UserType.Normal;
}