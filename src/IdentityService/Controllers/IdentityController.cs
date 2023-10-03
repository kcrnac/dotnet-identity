using IdentityService.Abstraction.Services;
using IdentityService.Dtos;
using IdentityService.Entities;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityService.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public IdentityController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!signInResult.Succeeded)
        {
            return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));
        }

        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        var response = new UserDto
        {
            Email = user.Email,
            Token = _tokenService.GenerateToken(user, roles),
            FullName = user.FullName
        };

        return Ok(response);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
        {
            return BadRequest(new ApiValidationErrorResponse(new[] { "Email address is in use" }));
        }

        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var identityResult = await _userManager.CreateAsync(user, registerDto.Password);
        if (!identityResult.Succeeded)
        {
            return BadRequest(new ApiResponse(400, "Could not create user account"));
        }

        await _userManager.AddToRoleAsync(user, UserRoles.User);

        var response = new UserDto
        {
            Email = user.Email,
            FullName = user.FullName
        };

        return Ok(response);
    }

    [HttpGet("authenticated")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<string> AuthenticatedOnly()
    {
        return Ok(new ResponseDto<string>("You're authenticated"));
    }

    [HttpGet("authorized")]
    [Authorize(Roles = $"{UserRoles.Administrator},{UserRoles.SuperUser}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<string> AuthorizedOnly()
    {
        var userRoles = User.Claims.Where(p => p.Type.Equals(ClaimTypes.Role)).Select(p => p.Value).ToArray();

        return Ok(new ResponseDto<string>($"You have the following roles: {string.Join(',', userRoles)}"));
    }

    [HttpGet("anonymous")]
    public ActionResult<string> Anonymous()
    {
        return Ok(new ResponseDto<string>($"This is publicly available"));
    }
}