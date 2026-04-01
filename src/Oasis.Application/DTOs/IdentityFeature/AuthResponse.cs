
namespace Oasis.Application.DTOs.IdentityFeature;


/// <summary>
/// 认证响应 DTO，包含访问令牌、刷新令牌及其过期时间等信息
/// </summary>
/// <param name="AccessToken"></param>
/// <param name="AccessTokenExpiresAtUtc"></param>
/// <param name="RefreshToken"></param>
/// <param name="RefreshTokenExpiresAtUtc"></param>
public record AuthResponse(
    string AccessToken,
    DateTime AccessTokenExpiresAtUtc,
    string RefreshToken,
    DateTime RefreshTokenExpiresAtUtc);