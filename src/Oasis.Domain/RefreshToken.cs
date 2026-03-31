namespace Oasis.Domain;


public class RefreshToken
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;

    public string TokenHash { get; set; } = default!;

    public DateTime ExpiresAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? RevokedAtUtc { get; set; }

    public DateTime? UsedAtUtc { get; set; } // 防重放

    public bool IsActive => RevokedAtUtc == null && UsedAtUtc == null && DateTime.UtcNow < ExpiresAtUtc;

}