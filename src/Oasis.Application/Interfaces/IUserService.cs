

using FluentResults;
using Oasis.Application.DTOs.IdentityFeature;

namespace Oasis.Application.Interfaces;

public interface IUserService
{
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="registerRequest"></param>
    /// <returns></returns>
    public Task<Result<AuthResponse>> CreateUserAsync(RegisterRequest registerRequest);


    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    public Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest);


    [Obsolete("Only for testing purposes, not recommended for production use.")]
    public Task<Result> DeleteUserAsync(string userId);

}