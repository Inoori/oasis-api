using Microsoft.EntityFrameworkCore;


namespace Oasis.Infrastructure.Persistence;

public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("guests");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(g => g.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(g => g.FullName)
            .HasColumnName("fullName")
            .HasMaxLength(100);

        builder.Property(g => g.Email)
            .HasColumnName("email")
            .HasMaxLength(100);

        builder.Property(g => g.Nationality)
            .HasColumnName("nationality")
            .HasMaxLength(50);

        builder.Property(g => g.CountryFlag)
            .HasColumnName("countryFlag")
            .HasMaxLength(200);

        builder.Property(g => g.NationalID)
            .HasColumnName("nationalID")
            .HasMaxLength(50);

    }
}
