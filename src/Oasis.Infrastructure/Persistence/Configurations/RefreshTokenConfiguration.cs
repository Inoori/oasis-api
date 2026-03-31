

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oasis.Domain;

namespace Oasis.Infrastructure.Persistence.Configurations;


public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {

        builder.Ignore(rt => rt.IsActive);

        builder.Property(rt => rt.Id)
           .ValueGeneratedOnAdd()
           .HasValueGenerator<GuidV7ValueGenerator>();

        builder.Property(rt => rt.UserId)
            .HasMaxLength(450); // IdentityUser 的主键长度默认为 450

        builder.Property(rt => rt.TokenHash)
            .HasMaxLength(64); // SHA256 哈希长度为 64 字符

        builder.Property(rt => rt.ExpiresAtUtc);

        builder.Property(rt => rt.CreatedAtUtc)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();

        builder.Property(rt => rt.RevokedAtUtc);


        builder.HasIndex(rt => rt.TokenHash)
            .IsUnique();
    }
}