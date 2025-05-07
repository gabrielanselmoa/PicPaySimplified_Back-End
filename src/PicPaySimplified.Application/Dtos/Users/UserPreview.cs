using System.Text.Json.Serialization;
using PicPaySimplified.Domain.Entities.Enums;

namespace PicPaySimplified.Application.Dtos.Users;

public record UserPreview
{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserType Type { get; init; }
}