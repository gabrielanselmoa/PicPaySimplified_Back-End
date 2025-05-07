using System.Text.Json.Serialization;
using PicPaySimplified.Domain.Entities.Enums;

namespace PicPaySimplified.Application.Dtos.Users;

public record UserResponse
{
    public string Id { get; init; } = null!;
    public string FullName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? Cpf { get; init; } = null!;
    public Decimal Balance { get; init; }
    public bool IsActive { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserType UserType { get; init; }
};