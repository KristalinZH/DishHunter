namespace DishHunter.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Restaurant;

    public class MenuItemSeedConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder
                .HasData(SeedMenuitems());
        }
        private IEnumerable<MenuItem> SeedMenuitems()
        {
            return new List<MenuItem>()
            {
                new MenuItem()
                {
                    Id=1,
                    Name="Бургер Crazy Me",
                    FoodCategory="Бургер",
                    Description="Описание на бургер",
                    Price=15.99M,
                    ImageUrl="https://dostavka.happy.bg/remote/files/images/40855/fit_600_376.png?rev=1663753137",
                    MenuId=1,
                    IsActive=true
                },
                new MenuItem()
                {
                    Id=2,
                    Name="Бургер Джак Даниелс",
                    FoodCategory="Бургер",
                    Description="Описание на бургер",
                    Price=16.99M,
                    ImageUrl="https://dostavka.happy.bg/remote/files/images/173243/fit_600_376.png?rev=1687678926",
                    MenuId=1,
                    IsActive=true
                },
                 new MenuItem()
                {
                    Id=3,
                    Name="Бърбън пица",
                    FoodCategory="Пица",
                    Description="Описание на пица",
                    Price=17.99M,
                    ImageUrl="https://gotvach.bg/files/1200x800/piza-tuna-domashna.webp",
                    MenuId=2,
                    IsActive=true
                },
                 new MenuItem()
                {
                    Id=4,
                    Name="Пица Маргарита",
                    FoodCategory="Пица",
                    Description="Описание на пица",
                    Price=11.99M,
                    ImageUrl="https://gotvach.bg/files/1200x800/pizza-margheritta1.webp",
                    MenuId=2,
                    IsActive=true
                },
            };
        }
    }
}
