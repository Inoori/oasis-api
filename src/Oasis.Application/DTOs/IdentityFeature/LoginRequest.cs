namespace Oasis.Application.DTOs.IdentityFeature;


public record LoginRequest(
    string Email,
    string Password);