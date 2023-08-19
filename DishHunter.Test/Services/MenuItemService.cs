namespace DishHunter.Test.Services
{
    using BaseTest;
    using DishHunter.Services.Data.Models.Enums;
    using DishHunter.Services.Data.Models.MenuItem;
    using static Common.NotificationMessagesConstants;
    [TestFixture]
    internal class MenuItemService:BaseTestClass
    {
        [Test]
        public async Task AddMenuItemsByMenuIdResult()
        {
            int menuId = 1;
            var mis = new List<MenuItemExcelTransferModel>()
            {
                new MenuItemExcelTransferModel()
                {
                    Name = "Test",
                    ImageUrl = "test",
                    FoodCategory = "test",
                    Price = 2,
                    Description="test"              
                }
            };

            var result = await menuItemService.AddMenuItemsByMenuIdAsync(mis, menuId);

            Assert.That(result, Is.EqualTo(SuccessfullyAddedMenuItems));
        }
        [Test]
        public async Task CreateMenuItemResult()
        {
            var menuItemModel = new MenuItemPostTransferModel
            {
                Name = "Test",
                ImageUrl = "test",
                FoodCategory = "test",
                Price = 2,
                Description="test",
                MenuId=1
            };

            int menuItemId = await menuItemService.CreateMenuItemAsync(menuItemModel);

            Assert.That(menuItemId, Is.Not.EqualTo(0));
        }

        [Test]
        public async Task DeleteMenuItemsByIdResult()
        {
            int menuId = 1;

            await menuItemService.DeleteMenuItemsByMenuIdAsync(menuId);
            var result = await menuItemService.GetMenuItemsByMenuIdAsync(menuId);

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteMenuItemByIdResult()
        {
            int menuItemId = 1;
            await menuItemService.DeleteMenuItemByIdAsync(menuItemId);
            var query = new MenuItemQueryTransferModel()
            {
                SearchItem = null,
                Sorting = MenuItemSorting.None
            };
            query = await menuItemService.GetAllMenuItemsAsCardsAsync(query);
            Assert.That(query.MenuItems.Count(), Is.EqualTo(3));
        }
        
        [Test]
        public async Task DeleteMenuItemsByMenusIdRangeResult()
        {
            List<int> mids = new List<int>() { 1, 2 };
            var query = new MenuItemQueryTransferModel()
            {
                SearchItem = null,
                Sorting = MenuItemSorting.None
            };
            await menuItemService.DeleteMenuItemsByMenusIdRangeAsync(mids);
            query = await menuItemService.GetAllMenuItemsAsCardsAsync(query);
            Assert.That(query.MenuItems.Count(), Is.EqualTo(0));
        }
        
        [Test]
        public async Task ExistsByIdTrueResult()
        {
            int id = 1;
            bool result = await menuItemService.ExistsByIdAsync(id);
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task ExistsByIdFalseResult()
        {
            int id = 6;
            bool result = await menuItemService.ExistsByIdAsync(id);
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public async Task EditMenuItemByIdResult()
        {
            int id = 1;
            var mi = await menuItemService.GetMenuItemForEditByIdAsync(id);
            mi.Description = "Raboti";
            await menuItemService.EditMenuItemByIdAsync(id, mi);
            mi= await menuItemService.GetMenuItemForEditByIdAsync(id);
            Assert.That(mi.Description, Is.EqualTo("Raboti"));
        }
        
        [Test]
        public async Task GetAllMenuItemsResult()
        {
            var query = new MenuItemQueryTransferModel()
            {
                SearchItem = null,
                Sorting = MenuItemSorting.None
            };
            query = await menuItemService.GetAllMenuItemsAsCardsAsync(query);
            Assert.That(query.MenuItems.Count(), Is.EqualTo(4));
        }

        [Test]
        public async Task GetMenuItemDetailsByIdResult()
        {
            int id = 1;
            var result = await menuItemService.GetMenuItemDetailsByIdAsync(id);
            Assert.That(result.Name,Is.EqualTo("Чийзбургер"));
            Assert.That(result.Description,Is.EqualTo("Най-вкусния чийзбугер някога"));
            Assert.That(result.FoodCategory, Is.EqualTo("Големи бургери"));
            Assert.That(result.Price,Is.EqualTo(14.90M));
            Assert.That(result.ImageUrl,Is.EqualTo("https://s7d1.scene7.com/is/image/mcdonalds/DC_202006_0003_Cheeseburger_StraightBun_832x472:1-3-product-tile-desktop?wid=765&hei=472&dpr=off"));
            Assert.That(result.Menu,Is.EqualTo("Основно"));
            Assert.That(result.Brand,Is.EqualTo("Gosho brand"));
        }
        
        [Test]
        public async Task GetMenuItemForEditByIdResult()
        {
            int id = 1;
            var result = await menuItemService.GetMenuItemForEditByIdAsync(id);
            Assert.That(result.Name, Is.EqualTo("Чийзбургер"));
            Assert.That(result.Description, Is.EqualTo("Най-вкусния чийзбугер някога"));
            Assert.That(result.FoodCategory, Is.EqualTo("Големи бургери"));
            Assert.That(result.Price, Is.EqualTo(14.90M));
            Assert.That(result.ImageUrl, Is.EqualTo("https://s7d1.scene7.com/is/image/mcdonalds/DC_202006_0003_Cheeseburger_StraightBun_832x472:1-3-product-tile-desktop?wid=765&hei=472&dpr=off"));
        }

        [Test]
        public async Task GetMenuItemsByMenuIdResult()
        {
            int id = 1;
            var result = await menuItemService.GetMenuItemsByMenuIdAsync(id);
            Assert.That(result.Count(), Is.EqualTo(2));         
        }
        [Test]
        public async Task GetOwnersMenuItemsByOwnerIdResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            var result = await menuItemService.GetOwnersMenuItemsByOwnerIdAsync(ownerId);
            Assert.That(result.Count(), Is.EqualTo(4));
        }

        [Test]
        public async Task MenuItemOwnedByOwnerByMenuItemIdAndOwnerIdTrueResult ()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            int id = 1;
            bool result = await menuItemService.MenuItemOwnedByOwnerByMenuItemIdAndOwnerIdAsync(id,ownerId);
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task MenuItemOwnedByOwnerByMenuItemIdAndOwnerIdFalseResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822c";
            int id = 1;
            bool result = await menuItemService.MenuItemOwnedByOwnerByMenuItemIdAndOwnerIdAsync(id, ownerId);
            Assert.That(result, Is.EqualTo(false));
        }
    }
}
