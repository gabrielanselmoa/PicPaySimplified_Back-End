using System.Text.Json.Serialization;

namespace PicPaySimplified.Domain.Entities;

public class HttpAuthorization
{
    [JsonPropertyName("authorization")]
    public bool Authorization { get; set; }
}