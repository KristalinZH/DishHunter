namespace DishHunter.Test.Services.BaseTest
{
    using System.Net.Http;
    using Data;
    using Data.Models.Restaurant;
    using Data.Models.Account;
    using Mock;
    using DishHunter.Services.Data.Interfaces;
    using DishHunter.Services.Data;
    using Microsoft.EntityFrameworkCore;

    [TestFixture]
    internal class BaseTestClass
    {
        protected ApplicationDbContext dbContext;
        protected IBrandService brandService;
        protected IRestaurantService restaurantService;
        protected IMenuService menuService;
        protected IMenuItemService menuItemService;
        protected ICategoryService categoryService;
        protected ISettlementService settlementService;
        protected IGeocodingService geocodingService;
        protected IRestaurantOwnerService ownerService;
        private HttpClient httpClient;
        [SetUp]
        public void SetUpBase()
        {
            dbContext = DatabaseMock.Instance;
            httpClient = new HttpClient();
            geocodingService = new GeocodingService(httpClient);
            categoryService = new CategoryService(dbContext);
            settlementService = new SettlementService(dbContext);
            menuItemService = new MenuItemService(dbContext);
            ownerService = new RestaurantOwnerService(dbContext);
            menuService = new MenuService(dbContext, menuItemService);
            restaurantService = new RestaurantService(dbContext, settlementService, categoryService, geocodingService);
            brandService = new BrandService(dbContext, restaurantService, menuService);
            SeedDatabase();
        }
        private async void SeedDatabase()
        {
            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id=Guid.Parse("33f33961-b87f-4398-ba3a-e69561a9d346"),
                    UserName="Gosho",
                    PhoneNumber="0996",
                    FirstName="Gosho",
                    LastName="Goshov",
                    Email="gosho@goshomail.com",
                    PasswordHash="AQAAAAEAACcQAAAAENyIRx81wcIfbeRfVJZfOzavk2ys/fSsfRu5oNJtolGwTJZCgBmvrasA7eLVJ21+gw=="
                },
                new ApplicationUser()
                {
                    Id=Guid.Parse("d740d68c-ba8e-448c-b2e4-f0db5b60c887"),
                    UserName="Pesho",
                    PhoneNumber="0997",
                    FirstName="Pesho",
                    LastName="Peshov",
                    Email="pesho@peshomail.com",
                    PasswordHash="AQAAAAEAACcQAAAAENyIRx81wcIfbeRfVJZfOzavk2ys/fSsfRu5oNJtolGwTJZCgBmvrasA7eLVJ21+gw=="
                },
                new ApplicationUser()
                {
                    Id=Guid.Parse("d740d68c-ba8e-448c-b2e4-f0db5b60c888"),
                    UserName="Mincho",
                    PhoneNumber="0998",
                    FirstName="Mincho",
                    LastName="Minchov",
                    Email="mincho@michov.com",
                    PasswordHash="AQAAAAEAACcQAAAAENyIRx81wcIfbeRfVJZfOzavk2ys/fSsfRu5oNJtolGwTJZCgBmvrasA7eLVJ21+gw=="
                },
                 new ApplicationUser()
                {
                    Id=Guid.Parse("d740d68c-ba8e-448c-b2e4-f0db5b60c889"),
                    UserName="Nasko",
                    FirstName="Nasko",
                    LastName="Naskov",
                    Email="nasko@naskov.com",
                    PasswordHash="AQAAAAEAACcQAAAAENyIRx81wcIfbeRfVJZfOzavk2ys/fSsfRu5oNJtolGwTJZCgBmvrasA7eLVJ21+gw=="
                }
            };
            List<RestaurantOwner> owners = new List<RestaurantOwner>()
            {
                new RestaurantOwner()
                {
                    Id=Guid.Parse("2618f422-fb08-42a7-bd9c-ffc4d311822a"),
                    UserId=users[0].Id,
                    IsActive=true
                },
                new RestaurantOwner()
                {
                    Id=Guid.Parse("2618f422-fb08-42a7-bd9c-ffc4d311822b"),
                    UserId=users[1].Id,
                    IsActive=true
                },
                 new RestaurantOwner()
                {
                    Id=Guid.Parse("2618f422-fb08-42a7-bd9c-ffc4d311822c"),
                    UserId=users[2].Id,
                    IsActive=true
                },
            };
            users[0].RestaurantOwnerId = owners[0].Id;
            users[1].RestaurantOwnerId = owners[1].Id;
            users[2].RestaurantOwnerId = owners[2].Id;

            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Id=1,
                    CategoryName="Ресторант",
                    IsActive=true
                },
                new Category()
                {
                    Id=2,
                    CategoryName="Бирария",
                    IsActive=true
                }
            };
            List<Settlement> settlements = new List<Settlement>()
            {
                new Settlement()
                {
                    Id=1,
                    Region="обл.Бургас",
                    SettlementName="гр.Бургас",
                    IsActive=true
                },
                new Settlement()
                {
                    Id=2,
                    Region="обл.София",
                    SettlementName="гр.София",
                    IsActive=true
                }
            };
            List<Brand> brands = new List<Brand>()
            {
                new Brand()
                {
                    Id=Guid.Parse("15d93c12-9a8c-40ce-a6ae-b4d7c980d707"),
                    BrandName="Gosho brand",
                    Description="Nai gotinata veriga za burzo hranene v burgas",
                    LogoUrl="https://lh5.googleusercontent.com/p/AF1QipNY3AIc5MD-tXpqXou00G2HougLrCEBUolx0Y8P=w260-h175-n-k-no",
                    WebsiteUrl="https://mcdonalds.bg/en/",
                    RestaurantOwnerId=owners[0].Id,
                    IsActive=true
                },
                 new Brand()
                {
                    Id=Guid.Parse("15d93c12-9a8c-40ce-a6ae-b4d7c980d708"),
                    BrandName="Gosho brand",
                    Description="Nai gotinata veriga za burzo hranene v burgas",
                    LogoUrl="https://lh5.googleusercontent.com/p/AF1QipNY3AIc5MD-tXpqXou00G2HougLrCEBUolx0Y8P=w260-h175-n-k-no",
                    WebsiteUrl="https://mcdonalds.bg/en/",
                    RestaurantOwnerId=owners[0].Id,
                    IsActive=true
                },
                  new Brand()
                {
                    Id=Guid.Parse("15d93c12-9a8c-40ce-a6ae-b4d7c980d709"),
                    BrandName="Gosho brand",
                    Description="Nai gotinata veriga za burzo hranene v burgas",
                    LogoUrl="https://lh5.googleusercontent.com/p/AF1QipNY3AIc5MD-tXpqXou00G2HougLrCEBUolx0Y8P=w260-h175-n-k-no",
                    WebsiteUrl="https://mcdonalds.bg/en/",
                    RestaurantOwnerId=owners[0].Id,
                    IsActive=true
                },
                   new Brand()
                {
                    Id=Guid.Parse("15d93c12-9a8c-40ce-a6ae-b4d7c980d710"),
                    BrandName="Gosho brand",
                    Description="Nai gotinata veriga za burzo hranene v burgas",
                    LogoUrl="https://lh5.googleusercontent.com/p/AF1QipNY3AIc5MD-tXpqXou00G2HougLrCEBUolx0Y8P=w260-h175-n-k-no",
                    WebsiteUrl="https://mcdonalds.bg/en/",
                    RestaurantOwnerId=owners[0].Id,
                    IsActive=true
                },
                new Brand()
                {
                    Id=Guid.Parse("15d93c12-9a8c-40ce-a6ae-b4d7c980d711"),
                    BrandName="Gosho brand",
                    Description="Nai gotinata veriga za burzo hranene v burgas",
                    LogoUrl="https://lh5.googleusercontent.com/p/AF1QipNY3AIc5MD-tXpqXou00G2HougLrCEBUolx0Y8P=w260-h175-n-k-no",
                    WebsiteUrl="https://mcdonalds.bg/en/",
                    RestaurantOwnerId=owners[1].Id,
                    IsActive=true
                }
            };
            List<Menu> menus = new List<Menu>()
            {
                new Menu()
                {
                    Id=1,
                    MenuType="Основно",
                    FoodType="Бургери",
                    Description="Най-добрите бургери в България",
                    BrandId=brands[0].Id,
                    IsActive=true
                },
                new Menu()
                {
                    Id=2,
                    MenuType="Лятно",
                    FoodType="Плодово",
                    Description="Най-сочните плодове в България",
                    BrandId=brands[0].Id,
                    IsActive=true
                }
            };
            List<MenuItem> menuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Id=1,
                    Name="Чийзбургер",
                    FoodCategory="Големи бургери",
                    Description="Най-вкусния чийзбугер някога",
                    Price=14.90M,
                    ImageUrl="https://s7d1.scene7.com/is/image/mcdonalds/DC_202006_0003_Cheeseburger_StraightBun_832x472:1-3-product-tile-desktop?wid=765&hei=472&dpr=off",
                    MenuId=1,
                    IsActive=true
                },
                new MenuItem()
                {
                    Id=2,
                    Name="Веган бургер",
                    FoodCategory="Малки бургери",
                    Description="Най-вкусния веган бургер някога",
                    Price=19.90M,
                    ImageUrl="https://www.wellplated.com/wp-content/uploads/2016/03/Black-Bean-Vegan-Burger-Recipe.jpg",
                    MenuId=1,
                    IsActive=true
                },
                new MenuItem()
                {
                    Id=3,
                    Name="Динено смути",
                    FoodCategory="Смути",
                    Description="Най-вкусното смути някога",
                    Price=5.90M,
                    ImageUrl="https://i2.wp.com/www.downshiftology.com/wp-content/uploads/2023/01/Cranberry-Smoothie-4.jpg",
                    MenuId=2,
                    IsActive=true
                },
                new MenuItem()
                {
                    Id=4,
                    Name="Фреш портокал",
                    FoodCategory="Натурални сокове",
                    Description="Най-вкусният сок някога",
                    Price=8.90M,
                    ImageUrl="https://www.alphafoodie.com/wp-content/uploads/2020/11/Orange-Juice-1-of-1.jpeg",
                    MenuId=2,
                    IsActive=true
                }
            };
            List<Restaurant> restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Id=Guid.Parse("3a77e8ec-2d43-4eff-9f3a-822a2d7b8be3"),
                    Name="При Гошо на бургер",
                    PhoneNumber="+35912345678",
                    Address="Сан Стефано 62",
                    ImageUrl="https://www.bfu.bg/uploads/sliders/slide_12.jpg",
                    BrandId=brands[0].Id,
                    SettlementId=settlements[0].Id,
                    Latitude=42.5029812M,
                    Longitude=27.4701599M,
                    CategoryId=categories[0].Id,
                    IsActive=true
                },
                new Restaurant()
                {
                    Id=Guid.Parse("d000b0d1-b049-47ea-9551-b6cc8e3dbec2"),
                    Name="При Гошо на плод",
                    PhoneNumber="+35912345678",
                    Address="Цар Освободител 15",
                    ImageUrl="https://upload.wikimedia.org/wikipedia/commons/c/ca/Sofia_University_panorama_2.jpg",
                    BrandId=brands[0].Id,
                    SettlementId=settlements[1].Id,
                    Latitude=42.6935092M,
                    Longitude=23.335316870417927M,
                    CategoryId=categories[1].Id,
                    IsActive=true
                }
            };
            await dbContext.Users.AddRangeAsync(users);
            await dbContext.RestaurantOwners.AddRangeAsync(owners);
            await dbContext.Categories.AddRangeAsync(categories);
            await dbContext.Settlements.AddRangeAsync(settlements);
            await dbContext.MenuItems.AddRangeAsync(menuItems);
            await dbContext.Menus.AddRangeAsync(menus);
            await dbContext.Restaurants.AddRangeAsync(restaurants);
            await dbContext.Brands.AddRangeAsync(brands);
            await dbContext.SaveChangesAsync();
        }
        [TearDown]
        public async Task DropDb()
        {
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
