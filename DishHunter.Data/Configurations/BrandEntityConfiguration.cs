namespace DishHunter.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Restaurant;

    public class BrandEntityConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder
                .Property(b => b.IsActive)
                .HasDefaultValue(true);
            builder
                .HasMany(b => b.Menus)
                .WithOne(m => m.Brand)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasMany(b => b.Restaurants)
                .WithOne(r => r.Brand)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(b => b.RestaurantOwner)
                .WithMany(ro => ro.OwnedBrands)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
