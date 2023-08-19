namespace DishHunter.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Restaurant;

    public class MenuSeedConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder
                .HasData(SeedMenus());
        }
        private IEnumerable<Menu> SeedMenus()
        {
            return new List<Menu>()
            {
                new Menu()
                {
                    Id=1,
                    MenuType="Основно",
                    FoodType="Интернационална",
                    Description="Описание на меню",
                    BrandId=Guid.Parse("7f69f846-bc9c-4058-9c84-36ce93e7933d"),
                    IsActive=true
                },
                new Menu()
                {
                    Id=2,
                    MenuType="Пици",
                    FoodType="Италианска",
                    Description="Описание на меню",
                    BrandId=Guid.Parse("b49ffdc5-5442-4c7e-8733-741e2763ed5d"),
                    IsActive=true
                }
            };
        }
    }
}
