using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oasis.Application.DTOs.UserFeature;
using Oasis.Domain;
using Oasis.Infrastructure.Persistence;

namespace Oasis.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class UsersController(UserManager<User> userManager) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(string userId, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.Select(user => new
        {
            user.Id,
            user.UserName,
            user.Email,
            user.Avatar,
        }).FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null) return NotFound();

        return Ok(user);
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