namespace DishHunter.Test.Services
{
    using BaseTest;
    using DishHunter.Services.Data.Models.Menu;
    using DishHunter.Services.Data.Models.MenuItem;
    using static Common.NotificationMessagesConstants;
    internal class MenuService:BaseTestClass
    {
        [Test]
        public async Task AddMenusByBrandIdResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            var menus = new List<MenuExcelTransferModel>()
            {
                new MenuExcelTransferModel()
                {
                    FoodType="test",
                    MenuType="test",
                    Description="test",
                    MenuItems=new List<MenuItemExcelTransferModel>()
                    {
                        new MenuItemExcelTransferModel()
                        {
                            Description="test",
                            FoodCategory="test",
                            ImageUrl="test",
                            Name="test",
                            Price=1
                        }
                    }
                }
            };
            var result = await menuService.AddMenusByBrandIdAsync(menus, brandId);

            Assert.That(result, Is.EqualTo(SuccessfullyAddedMenus));
        }
        [Test]
        public async Task AnyMenuOwnedByOwnerByOwnerIdTrueResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            bool result = await menuService.AnyMenuOwnedByOwnerByOwnerIdAsync(ownerId);

            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task AnyMenuOwnedByOwnerByOwnerIdFalseResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822c";
            bool result = await menuService.AnyMenuOwnedByOwnerByOwnerIdAsync(ownerId);

            Assert.That(result, Is.EqualTo(false));
        }
        
        [Test]
        public async Task CreateMenuResult()
        {

            var menuModel = new MenuPostTransferModel
            {
                MenuType="test",
                Description="test",
                FoodType="test",
                BrandId= "2618f422-fb08-42a7-bd9c-ffc4d311822a"
            };

            int menu = await menuService.CreateMenuAsync(menuModel);

            Assert.That(menu, Is.Not.EqualTo(0));
        }
        
        [Test]
        public async Task DeleteMenuByIdResult()
        {
            int menuId = 1;
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            await menuService.DeleteMenuByIdAsync(menuId);
            var menus = await menuService.GetMenusByBrandIdAsync(brandId);
            Assert.That(menus.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteMenusByBrandIdResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            await menuService.DeleteMenusByBrandIdAsync(brandId);
            var menus = await menuService.GetMenusByBrandIdAsync(brandId);
            Assert.That(menus.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task EditMenuByIdResult()
        {
            int menuId = 1;
            var menu = await menuService.GetMenuForEditByIdAsync(menuId);
            menu.FoodType = "test";
            await menuService.EditMenuByIdAsync(menuId, menu);
            menu = await menuService.GetMenuForEditByIdAsync(menuId);
            Assert.That(menu.FoodType, Is.EqualTo("test"));
        }

        [Test]
        public async Task ExistsByIdTrueResult()
        {
            int menu = 1;
            bool result = await menuService.ExistsByIdAsync(menu);
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task ExistsByIdFalseResult()
        {
            int menu = 6;
            bool result = await menuService.ExistsByIdAsync(menu);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task GetMenuDetailsByIdResult()
        {
            int menu = 1;
            var result = await menuService.GetMenuDetailsByIdAsync(menu);
            Assert.That(result.MenuType, Is.EqualTo("Основно"));
            Assert.That(result.FoodType, Is.EqualTo("Бургери"));
            Assert.That(result.Brand, Is.EqualTo("Gosho brand"));
            Assert.That(result.Description, Is.EqualTo("Най-добрите бургери в България"));
            Assert.That(result.MenuItems.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetMenuForEditByIdResult()
        {
            int menu = 1;
            var result = await menuService.GetMenuForEditByIdAsync(menu);
            Assert.That(result.MenuType, Is.EqualTo("Основно"));
            Assert.That(result.FoodType, Is.EqualTo("Бургери"));
            Assert.That(result.Description, Is.EqualTo("Най-добрите бургери в България"));
        }

        [Test]
        public async Task GetMenusByBrandIdResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            var result = await menuService.GetMenusByBrandIdAsync(brandId);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetMenusForSelectByOwnerIdResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            var result = await menuService.GetMenusForSelectByOwnerIdAsync(ownerId);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetOwnerMenusByOwnerIdResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            var result = await menuService.GetOwnerMenusByOwnerIdAsync(ownerId);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task MenuOwnedByOwnerByMenuIdAndOwnerIdTrueResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            int menuId = 1;
            var result = await menuService.MenuOwnedByOwnerByMenuIdAndOwnerIdAsync(menuId, ownerId);
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task MenuOwnedByOwnerByMenuIdAndOwnerIdFalseResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822c";
            int menuId = 1;
            var result = await menuService.MenuOwnedByOwnerByMenuIdAndOwnerIdAsync(menuId, ownerId);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task DeleteMenusByBrandBrandsIdRangeResult()
        {
            List<Guid> ids = new List<Guid>()
            {
                Guid.Parse("15d93c12-9a8c-40ce-a6ae-b4d7c980d707")
            };
            await menuService.DeleteMenusByBrandBrandsIdRangeAsync(ids);
            var result = await menuService.GetMenusByBrandIdAsync(ids[0].ToString());
            Assert.That(result.Count(), Is.EqualTo(0));
        }
    }
}
