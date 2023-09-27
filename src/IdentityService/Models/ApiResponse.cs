using System.Text.Json.Serialization;

namespace IdentityService.Models;

public record ApiResponse(
    int StatusCode,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string Message = null);