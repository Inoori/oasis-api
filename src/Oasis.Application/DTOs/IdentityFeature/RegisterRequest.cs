namespace Oasis.Application.DTOs.IdentityFeature;


public record RegisterRequest(string Email, string Password, string UserName);