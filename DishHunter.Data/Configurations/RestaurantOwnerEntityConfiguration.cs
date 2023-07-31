namespace DishHunter.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Account;

    public class RestaurantOwnerEntityConfiguration : IEntityTypeConfiguration<RestaurantOwner>
    {
        public void Configure(EntityTypeBuilder<RestaurantOwner> builder)
        {
            builder
                .Property(ro => ro.IsActive)
                .HasDefaultValue(true);
            builder
                .HasMany(ro => ro.OwnedBrands)
                .WithOne(b => b.RestaurantOwner)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(ro => ro.User)
                .WithOne(u => u.RestaurantOwner)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
