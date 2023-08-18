namespace DishHunter.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Account;

    public class RestaurantOwnerSeedConfiguration : IEntityTypeConfiguration<RestaurantOwner>
    {
        public void Configure(EntityTypeBuilder<RestaurantOwner> builder)
        {
            builder
                .HasData(SeedOwner());
        }
        private RestaurantOwner SeedOwner()
        {
            return new RestaurantOwner()
            {
                Id= Guid.Parse("10a2102e-f116-4b81-b0a0-32b0d1022cb9"),
                UserId= Guid.Parse("aadb31cc-2d98-4864-84f7-127ea6097123"),
                IsActive =true
            };
        }
    }
}
