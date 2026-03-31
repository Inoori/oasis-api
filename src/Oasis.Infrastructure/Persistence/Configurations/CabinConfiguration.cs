using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oasis.Domain;

namespace Oasis.Infrastructure.Persistence.Configurations;

/// <summary>
/// Cabin entity configuration
/// </summary>
public class CabinConfiguration : IEntityTypeConfiguration<Cabin>
{
    public void Configure(EntityTypeBuilder<Cabin> builder)
    {

        builder.ToTable("cabins");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(100);

        builder.Property(c => c.MaxCapacity)
            .HasColumnName("maxCapacity");

        builder.Property(c => c.RegularPrice)
            .HasColumnName("regularPrice");

        builder.Property(c => c.Discount)
            .HasColumnName("discount");

        builder.Property(c => c.Description)
            .HasColumnName("description");

        builder.Property(c => c.Image)
            .HasColumnName("image");
    }
}