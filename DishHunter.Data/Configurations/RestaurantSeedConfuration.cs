namespace DishHunter.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Restaurant;

    public class RestaurantSeedConfuration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder
                .HasData(SeedRestaurants());
        }
        private IEnumerable<Restaurant> SeedRestaurants()
        {
            return new List<Restaurant>()
            {
                new Restaurant()
                {
                    Id=Guid.Parse("f7aa574f-0eb5-4256-8754-a9b9c6fb1fd8"),
                    Name="Happy Бургас",
                    PhoneNumber="+3591112222",
                    BrandId=Guid.Parse("7f69f846-bc9c-4058-9c84-36ce93e7933d"),
                    ImageUrl="https://rezzo.bg/files/images/9484/fit_431_304.jpg",
                    CategoryId=1,
                    SettlementId=552,
                    Address="ж.к. Лазур 41",
                    Latitude=42.5067369M,
                    Longitude=27.4778636M,
                    IsActive=true
                },
                new Restaurant()
                {
                    Id=Guid.Parse("f7aa574f-0eb5-4256-8754-a9b9c6fb1fd9"),
                    Name="Доминос София",
                    PhoneNumber="+3592221111",
                    BrandId=Guid.Parse("b49ffdc5-5442-4c7e-8733-741e2763ed5d"),
                    ImageUrl="https://imgrabo.com/pics/guide/900x600/20210311142613_91734.jpeg",
                    CategoryId=1,
                    SettlementId=4368,
                    Address="Александър Стамболийски",
                    Latitude=42.7071623M,
                    Longitude=23.1469427M,
                    IsActive=true
                }
            };
        }
    }
}
