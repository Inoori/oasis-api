using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Oasis.Application.DTOs.IdentityFeature;
using Oasis.Domain;

namespace Oasis.Infrastructure.Services.Identity;


/// <summary>
/// JWT 令牌服务
/// </summary>
/// <param name="configuration"></param>
/// <param name="logger"></param>
/// <param name="userManager"></param>
public class TokenService(IConfiguration configuration, ILogger<TokenService> logger, UserManager<User> userManager)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<TokenService> _logger = logger;

    private readonly UserManager<User> _userManager = userManager;


    /// <summary>
    /// 生成 JWT 令牌
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<(string, DateTime)> CreateAccessTokenAsync(User user)
    {
        string key = _configuration["Jwt:SecretKey"]!;
        string issuer = _configuration["Jwt:Issuer"]!;
        string audience = _configuration["Jwt:Audience"]!;
        double expiresInMinutes = Convert.ToDouble(_configuration["Jwt:AccessTokenExpiresInMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Id),
            new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email, user.Email!),
            new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty)
        };

        // 添加角色声明
        foreach (var role in await _userManager.GetRolesAsync(user))
            claims.Add(new Claim(ClaimTypes.Role, role));

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expires,
            signingCredentials: credentials);


        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }

    /// <summary>
    /// 生成新的刷新令牌
    /// </summary>
    /// <returns></returns>
    public static string CreateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// 计算 SHA-256 哈希值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ComputeSha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}
