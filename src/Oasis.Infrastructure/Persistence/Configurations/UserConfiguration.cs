using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oasis.Domain;

namespace Oasis.Infrastructure.Persistence;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Avatar)
            .HasMaxLength(255); // 设置最大长度，确保不会超过数据库字段限制
    }
}
