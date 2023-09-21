using System.Text.Json.Serialization;

namespace IdentityService.Models
{
    public record ApiException(
        int StatusCode,
        string Message = null,
        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]string Description = null)
        : ApiResponse(StatusCode, Message);
}