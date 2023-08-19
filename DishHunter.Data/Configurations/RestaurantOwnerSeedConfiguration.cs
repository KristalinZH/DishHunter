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
        private IEnumerable<RestaurantOwner> SeedOwner()
        {
            return new List<RestaurantOwner>()
            {
                new RestaurantOwner()
                {
                    Id= Guid.Parse("62152f86-525b-454f-92c8-108cea75c239"),
                    UserId= Guid.Parse("b49d1805-e143-47ed-9b72-7761e20d6c88"),
                    IsActive =true
                },
                new RestaurantOwner()
                {
                    Id= Guid.Parse("62152f86-525b-454f-92c8-108cea75c240"),
                    UserId= Guid.Parse("9e9d933d-973a-433a-ada2-19e4a7d4a509"),
                    IsActive =true
                }
            };
        }
    }
}
