using System.ComponentModel.DataAnnotations;

namespace IdentityService.Dtos;

public class LoginDto
{
    [Required]
    [MinLength(6)]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}