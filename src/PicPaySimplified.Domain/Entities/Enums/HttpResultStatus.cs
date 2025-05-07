using System.Text.Json.Serialization;

namespace PicPaySimplified.Domain.Entities.Enums;

public enum HttpResultStatus
{
    [JsonPropertyName("success")]
    SUCCESS,
    [JsonPropertyName("failed")]
    FAILED,
}