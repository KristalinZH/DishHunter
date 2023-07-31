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
        }
    }
}
