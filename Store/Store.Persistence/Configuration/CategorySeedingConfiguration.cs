using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Persistence.Configuration
{
    public class CategorySeedingConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(100).IsRequired();

            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new Category { Id = 1, Title = "Electronics", Description = "Phones, laptops and accessories", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { Id = 2, Title = "Books", Description = "Printed and digital books", CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { Id = 3, Title = "Home", Description = "Furniture and home appliances", CreatedAt = seedDate, UpdatedAt = seedDate });
        }
    }
}
