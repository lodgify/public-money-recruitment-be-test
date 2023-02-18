using System.Text.Json.Serialization;

namespace Models.Models;

/// <summary>
/// Model for swagger api documentation.
/// </summary>
public sealed class SwaggerErrorMessageModel
{
    public SwaggerErrorMessageModel(string message)
    {
        Message = message;
    }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
