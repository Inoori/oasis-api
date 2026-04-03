
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oasis.Application.DTOs.IdentityFeature;
using Oasis.Application.Interfaces;
using Oasis.Domain;
using Oasis.Infrastructure.Persistence;

namespace Oasis.Infrastructure.Services.Identity;

/// <summary>
/// 用户服务实现类，负责处理用户相关的业务逻辑
/// </summary>
public class UserService(UserManager<User> userManager, SignInManager<User> signInManager, OasisDbContext db, ILogger<UserService> logger) : IUserService
{

    /// <summary>
    /// 注册用户
    /// </summary>
    /// <param name="registerRequest"></param>
    /// <returns></returns>
    public async Task<Result> RegisterUser(RegisterRequest registerRequest)
    {
        var user = new User { UserName = registerRequest.UserName, Email = registerRequest.Email };
        //Todo:前端密码加密后传输，需要先解密再创建用户
        var result = await userManager.CreateAsync(user, registerRequest.Password);

        if (result is not { Succeeded: true }) return Result.Fail(result.Errors.Select(e => e.Description));

        await signInManager.SignInAsync(user, isPersistent: false);

        return Result.Ok();
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    public async Task<Result> LoginAsync(LoginRequest loginRequest)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);

        if (user is null) return Result.Fail("user not found or password incorrect");

        //todo: 实现记住我功能，使用 Identity 的持久化 cookie 功能，并确保在前端正确处理 SameSite 和 Secure 属性以支持跨站点请求
        var result = await signInManager.PasswordSignInAsync(
            user.UserName!,
            loginRequest.Password,
            isPersistent: true,
            lockoutOnFailure: false);

        if (result is not { Succeeded: true }) return Result.Fail("user not found or password incorrect");

        return Result.Ok();
    }


    /// <summary>
    /// 用户登出
    /// </summary>
    /// <returns></returns>
    public async Task<Result> LogoutAsync()
    {
        await signInManager.SignOutAsync();
        return Result.Ok();
    }


    /// <summary>
    /// 刷新访问令牌和刷新令牌
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        return Result.Fail("Not implemented yet");
    }


    [Obsolete("Only for testing purposes, not recommended for production use.")]
    public async Task<Result> DeleteUserAsync(string userId)
    {
        //使用 transaction 来确保用户和相关刷新令牌的原子删除
        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return Result.Fail("用户不存在");

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded) return Result.Fail(result.Errors.Select(e => e.Description));

            var userRefreshTokens = db.RefreshTokens.Where(rt => rt.UserId == userId);
            db.RefreshTokens.RemoveRange(userRefreshTokens);
            await db.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            logger.LogError(ex, "Error occurred while deleting user with ID {UserId}", userId);
            return Result.Fail("An error occurred while deleting the user").WithError(ex.Message);
        }
    }
}