using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Oasis.Infrastructure.Persistence;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");

        // Primary Key
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();


        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(b => b.StartDate)
            .HasColumnName("startDate");

        builder.Property(b => b.EndDate)
            .HasColumnName("endDate");

        builder.Property(b => b.NumNights)
            .HasColumnName("numNights");

        builder.Property(b => b.NumGuests)
            .HasColumnName("numGuests");

        builder.Property(b => b.CabinPrice)
            .HasColumnName("cabinPrice");

        builder.Property(b => b.ExtrasPrice)
            .HasColumnName("extrasPrice");

        builder.Property(b => b.TotalPrice)
            .HasColumnName("totalPrice");

        builder.Property(b => b.Status)
            .HasColumnName("status")
            .HasDefaultValue(BookingStatus.UnConfirmed);

        builder.Property(b => b.HasBreakfast).HasColumnName("hasBreakfast");
        builder.Property(b => b.IsPaid).HasColumnName("isPaid");

        builder.Property(b => b.Observations).HasColumnName("observations")
            .HasMaxLength(500);

        // 显示指定外键
        builder.Property(b => b.CabinId)
            .HasColumnName("cabinID");

        builder.Property(b => b.GuestId)
            .HasColumnName("guestID");

        // Relationships
        builder.HasOne(b => b.Cabin)
            .WithMany()
            .HasForeignKey(b => b.CabinId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Guest)
            .WithMany()
            .HasForeignKey(b => b.GuestId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
