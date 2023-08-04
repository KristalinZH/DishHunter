namespace DishHunter.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Restaurant;

    public class RestaurantEntityConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder
                .Property(r => r.Longitude)
                .HasPrecision(9, 6);
            builder
                .Property(r => r.Latitude)
                .HasPrecision(9, 6);
            builder
                .Property(r => r.IsActive)
                .HasDefaultValue(true);
            builder
                .HasOne(r => r.Brand)
                .WithMany(b => b.Restaurants)
                .OnDelete(DeleteBehavior.Restrict);
            builder
               .HasOne(r => r.Settlement)
               .WithMany(s => s.Restaurants)
               .OnDelete(DeleteBehavior.Restrict);
            builder
               .HasOne(r => r.Category)
               .WithMany(c => c.Restaurants)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
