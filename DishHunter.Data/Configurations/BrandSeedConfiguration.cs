namespace DishHunter.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Restaurant;

    public class BrandSeedConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder
                .HasData(SeedBrands());
        }
        private IEnumerable<Brand> SeedBrands()
        {
            return new List<Brand>()
            {
                new Brand()
                {
                    Id=Guid.Parse("7f69f846-bc9c-4058-9c84-36ce93e7933d"),                 
                    BrandName="Happy",
                    Description="Описанието на Happy",
                    LogoUrl="https://happy.bg/assets/images/logo_pink_alt.svg",
                    WebsiteUrl="https://happy.bg/",
                    RestaurantOwnerId=Guid.Parse("62152f86-525b-454f-92c8-108cea75c239"),
                    IsActive=true
                },
                new Brand()
                {
                    Id=Guid.Parse("b49ffdc5-5442-4c7e-8733-741e2763ed5d"),
                    BrandName="Dominos",
                    Description="Описанието на Dominos",
                    LogoUrl="https://i.pinimg.com/736x/1b/ee/08/1bee08aa56544de70e0c6bffe4a944a4.jpg",
                    WebsiteUrl="https://www.dominos.bg/",
                    RestaurantOwnerId=Guid.Parse("62152f86-525b-454f-92c8-108cea75c240"),
                    IsActive=true
                }
            };
        }
    }
}
