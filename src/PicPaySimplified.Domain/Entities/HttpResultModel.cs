using System.Text.Json.Serialization;

namespace PicPaySimplified.Domain.Entities;

public class HttpResultModel<T>
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}