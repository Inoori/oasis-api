using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oasis.Application.DTOs.UserFeature;
using Oasis.Domain;

namespace Oasis.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(UserManager<User> userManager) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await userManager.GetUserAsync(User);   // User 是 ClaimsPrincipal

        if (user is null) return NotFound();

        return Ok(new
        {
            id = user.Id,
            userName = user.UserName,
            email = user.Email,
            avatar = user.Avatar
        });
    }


    [HttpPost("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest updateProfileRequest)
    {
        var user = await userManager.FindByIdAsync(updateProfileRequest.UserId);
        if (user is null) return NotFound();

        user.UserName = updateProfileRequest.UserName;

        var result = await userManager.UpdateAsync(user);

        if (result is not { Succeeded: true }) return BadRequest(result.Errors);

        return Ok();
    }
}