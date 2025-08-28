using System.Text.Json.Serialization;

namespace Vneed.Common.Models;

public class ApiResponse<T>
{
    [JsonPropertyOrder(0)]
    public bool Success { get; set; }

    [JsonPropertyOrder(1)]
    public long Timestamp { get; set; }

    [JsonPropertyOrder(2)]
    public string OperationId { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyOrder(3)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    [JsonPropertyOrder(4)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Result { get; set; }

    [JsonPropertyOrder(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ApiError? Error { get; set; }
}