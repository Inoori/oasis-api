
namespace Oasis.Application.DTOs.IdentityFeature;

/// <summary>
/// 认证响应对象
/// </summary>
/// <param name="token">JWT 令牌字符串</param>
/// <param name="expiration">JWT 令牌过期时间</param>
public class AuthResponse(string token, DateTime expiration)
{
    /// <summary>
    /// JWT 令牌字符串
    /// </summary>
    public string Token { get; init; } = token;

    /// <summary>
    /// JWT 令牌过期时间
    /// </summary>
    public DateTime Expiration { get; init; } = expiration;
}