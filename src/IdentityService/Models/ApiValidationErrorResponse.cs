namespace IdentityService.Models
{
    public record ApiValidationErrorResponse(IEnumerable<string> Errors) : ApiResponse(StatusCodes.Status400BadRequest);
}
