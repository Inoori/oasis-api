namespace Oasis.Application.DTOs.UserFeature;

public record UpdateProfileRequest(
    string UserId,
    string UserName
);