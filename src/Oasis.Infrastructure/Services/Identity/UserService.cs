
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
public class UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, TokenService tokenService, IConfiguration configuration, OasisDbContext db, ILogger<UserService> logger) : IUserService
{

    private readonly double _refreshTokenExpiresInDays = configuration.GetValue<double>("Jwt:RefreshTokenExpiresInDays", 7);

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

        var token = await IssueTokensAsync(user);

        return Result.Ok(token);
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

        var token = await IssueTokensAsync(user);
        return Result.Ok(token);
    }


    /// <summary>
    /// 刷新访问令牌和刷新令牌
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var refreshTokenHash = TokenService.ComputeSha256(request.RefreshToken);

        var storedToken = await db.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == refreshTokenHash);

        if (storedToken is null) return Result.Fail<AuthResponse>("Invalid refresh token");

        if (!storedToken.IsActive) return Result.Fail<AuthResponse>("Refresh token is expired or revoked");

        var user = await db.Users.FindAsync(storedToken.UserId);

        if (user == null) return Result.Fail<AuthResponse>("User not found");

        //删除旧 刷新token
        await db.RefreshTokens.Where(rt => rt.UserId == user.Id).ExecuteDeleteAsync();

        var newRefreshToken = TokenService.CreateRefreshToken();
        var refreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(_refreshTokenExpiresInDays);

        // 存储新 token 
        db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = TokenService.ComputeSha256(newRefreshToken),
            ExpiresAtUtc = refreshTokenExpiresAtUtc
        });

        await db.SaveChangesAsync();

        var (accessToken, accessTokenExpiresAtUtc) = await tokenService.CreateAccessTokenAsync(user);

        return Result.Ok(new AuthResponse(accessToken, accessTokenExpiresAtUtc, newRefreshToken, refreshTokenExpiresAtUtc));

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


    /// <summary>
    /// 颁发访问令牌和刷新令牌，并将刷新令牌存储在数据库中
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private async Task<AuthResponse> IssueTokensAsync(User user)
    {
        var (accessToken, accessTokenExpiresAtUtc) = await tokenService.CreateAccessTokenAsync(user);

        var refreshToken = TokenService.CreateRefreshToken();

        var refreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(_refreshTokenExpiresInDays);

        var refreshEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = TokenService.ComputeSha256(refreshToken),
            ExpiresAtUtc = refreshTokenExpiresAtUtc,
        };

        // 先删除旧的刷新令牌，确保每次登录后只有一个有效的刷新令牌
        await db.RefreshTokens.Where(rt => rt.UserId == user.Id).ExecuteDeleteAsync();

        // 存储新的刷新令牌
        db.RefreshTokens.Add(refreshEntity);
        await db.SaveChangesAsync();

        return new AuthResponse(
            accessToken,
            accessTokenExpiresAtUtc,
            refreshToken,
            refreshTokenExpiresAtUtc
        );
    }

}