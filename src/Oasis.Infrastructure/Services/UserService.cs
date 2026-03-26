
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Oasis.Application.DTOs.IdentityFeature;
using Oasis.Application.Interfaces;
using Oasis.Domain;

namespace Oasis.Infrastructure.Services;

/// <summary>
/// 用户服务实现类，负责处理用户相关的业务逻辑
/// </summary>
public class UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, TokenService tokenService) : IUserService
{

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="registerRequest"></param>
    /// <returns></returns>
    public async Task<Result<AuthResponse>> CreateUserAsync(RegisterRequest registerRequest)
    {
        var user = new User { UserName = registerRequest.UserName, Email = registerRequest.Email };
        //Todo:前端密码加密后传输，需要先解密再创建用户
        var result = await userManager.CreateAsync(user, registerRequest.Password);

        if (!result.Succeeded) return Result.Fail(result.Errors.Select(e => e.Description));

        var tokenResponse = await tokenService.GenerateToken(user);

        return Result.Ok(tokenResponse);
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, loginRequest.Password))
            return Result.Fail("user not found or password incorrect");

        var tokenResponse = await tokenService.GenerateToken(user);
        return Result.Ok(tokenResponse);
    }


    [Obsolete("Only for testing purposes, not recommended for production use.")]
    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return Result.Fail("用户不存在");

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded) return Result.Fail(result.Errors.Select(e => e.Description));

        return Result.Ok();
    }
}