using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Oasis.Application.DTOs.IdentityFeature;
using Oasis.Domain;

namespace Oasis.Infrastructure.Services;


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
    public async Task<AuthResponse> GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.UserName ?? string.Empty)
        };

        // 添加角色声明
        foreach (var role in await _userManager.GetRolesAsync(user))
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(
            Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"] ?? "60"));

        var securityTokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = creds
        };

        return new AuthResponse(token: new JsonWebTokenHandler().CreateToken(securityTokenDescriptor), expiration: expires);
    }

}
