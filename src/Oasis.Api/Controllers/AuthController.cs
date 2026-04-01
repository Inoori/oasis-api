using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Oasis.Application.DTOs.IdentityFeature;
using Oasis.Application.Interfaces;
namespace Oasis.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> CreateUser([FromBody] RegisterRequest request)
    {
        var result = await userService.CreateUserAsync(request);
        if (result.IsFailed) return result.ToActionResult();
        return CreatedAtAction(nameof(CreateUser), result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await userService.LoginAsync(request);
        if (result.IsFailed) return result.ToActionResult(StatusCodes.Status401Unauthorized);
        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await userService.RefreshTokenAsync(request);
        if (result.IsFailed) return result.ToActionResult(StatusCodes.Status401Unauthorized);
        return Ok(result.Value);
    }


    [HttpPost("delete/{userId}")]
    [Obsolete("Only for testing purposes, not recommended for production use.")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await userService.DeleteUserAsync(userId);
        if (result.IsFailed) return result.ToActionResult(StatusCodes.Status404NotFound);
        return NoContent();
    }
}