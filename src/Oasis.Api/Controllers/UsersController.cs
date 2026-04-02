using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oasis.Application.DTOs.UserFeature;
using Oasis.Domain;

namespace Oasis.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(UserManager<User> userManager) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GetUserById([FromBody] GetUserRequest getUserRequest, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.Select(user => new
        {
            user.Id,
            user.UserName,
            user.Email,
            user.Avatar,
        }).FirstOrDefaultAsync(u => u.Id == getUserRequest.UserId, cancellationToken);

        if (user == null) return NotFound();

        return Ok(user);
    }
}