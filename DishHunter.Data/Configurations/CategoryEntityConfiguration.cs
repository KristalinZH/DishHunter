namespace DishHunter.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Restaurant;

    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .Property(c => c.IsActive)
                .HasDefaultValue(true);
            builder
                .HasMany(c => c.Restaurants)
                .WithOne(r => r.Category)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasData(CategoryData());
        }
        private Category[] CategoryData()
        {
            return new Category[] 
            {
                new Category()
                {
                    Id=1,
                    CategoryName="Ресторант",
                },
                new Category()
                {
                    Id=2,
                    CategoryName="Закусвалня",
                },
                new Category()
                {
                    Id=3,
                    CategoryName="Бързо хранене/Junk food",
                },
                new Category()
                {
                    Id=4,
                    CategoryName="Сладкарница",
                },
                new Category()
                {
                    Id=5,
                    CategoryName="Кафене",
                },
                new Category()
                {
                    Id=6,
                    CategoryName="Бирария",
                }
            };
        }
    }
}
