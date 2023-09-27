using System.Text.Json.Serialization;

namespace IdentityService.Dtos;

public class UserDto
{
    public string Email { get; set; }

    public string FullName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Token { get; set; }
}