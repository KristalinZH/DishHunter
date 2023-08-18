namespace DishHunter.Test.Services
{

    using BaseTest;
    using DishHunter.Services.Data.Models.Restaurant;


    [TestFixture]
    internal class RestaurantService:BaseTestClass
    {
        [Test]
        public async Task AddRestaurantsByBrandIdOkayResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            var r = new List<RestaurantExcelTransferModel>()
            {
                new RestaurantExcelTransferModel()
                {
                    Name="test",
                    PhoneNumber="test",
                    ImageUrl="test",
                    CategoryName="Ресторант",
                    Region="Бургас",
                    SettlementName="Бургас",
                    Address="Сан Стефано 62"
                }
            };
            var result = await restaurantService.AddRestaurantsByBrandIdAsync(r, brandId);
            Assert.That(result.AreRestaurantsAddedSuccessfully, Is.EqualTo(true));           
        }
        [Test]
        public async Task AddRestaurantsByBrandIdInvalidCategoryResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            var r = new List<RestaurantExcelTransferModel>()
            {
                new RestaurantExcelTransferModel()
                {
                    Name="test",
                    PhoneNumber="test",
                    ImageUrl="test",
                    CategoryName="Нещо",
                    Region="Бургас",
                    SettlementName="Бургас",
                    Address="Сан Стефано 62"
                }
            };
            var result = await restaurantService.AddRestaurantsByBrandIdAsync(r, brandId);
            Assert.That(result.AreRestaurantsAddedSuccessfully, Is.EqualTo(false));
        }
        [Test]
        public async Task AddRestaurantsByBrandIdInvalidSettlementResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            var r = new List<RestaurantExcelTransferModel>()
            {
                new RestaurantExcelTransferModel()
                {
                    Name="test",
                    PhoneNumber="test",
                    ImageUrl="test",
                    CategoryName="Ресторант",
                    Region="Мадагаскар",
                    SettlementName="Бургас",
                    Address="Сан Стефано 62"
                }
            };
            var result = await restaurantService.AddRestaurantsByBrandIdAsync(r, brandId);
            Assert.That(result.AreRestaurantsAddedSuccessfully, Is.EqualTo(false));
        }
        [Test]
        public async Task AddRestaurantsByBrandIdTooInvalidAddressResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            var r = new List<RestaurantExcelTransferModel>()
            {
                new RestaurantExcelTransferModel()
                {
                    Name="test",
                    PhoneNumber="test",
                    ImageUrl="test",
                    CategoryName="Ресторант",
                    Region="Бургас",
                    SettlementName="Бургас",
                    Address="не позна"
                }
            };
            var result = await restaurantService.AddRestaurantsByBrandIdAsync(r, brandId);
            Assert.That(result.AreRestaurantsAddedSuccessfully, Is.EqualTo(false));
        }
        [Test]
        public async Task CreateRestaurantResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            var r = new RestaurantPostTransferModel()
            {
                Name = "test",
                PhoneNumber = "test",
                ImageUrl = "test",
                CategoryId=1,
                SettlementId=1,
                Address = "Сан Стефано 62",
                BrandId=brandId
            };
            var result = await restaurantService.CreateRestaurantAsync(r);
            Assert.That(result.IsRestaurantAdded, Is.EqualTo(true));
        }
        [Test]
        public async Task CreateRestaurantInvalidAddressResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            var r = new RestaurantPostTransferModel()
            {
                Name = "test",
                PhoneNumber = "test",
                ImageUrl = "test",
                CategoryId = 1,
                SettlementId = 1,
                Address = "не позна",
                BrandId = brandId
            };
            var result = await restaurantService.CreateRestaurantAsync(r);
            Assert.That(result.IsRestaurantAdded, Is.EqualTo(false));
        }
        [Test]
        public async Task DeleteRestaurantByIdResult()
        {
            string restaurantId = "3a77e8ec-2d43-4eff-9f3a-822a2d7b8be3";
            await restaurantService.DeleteRestaurantByIdAsync(restaurantId);
            var r = await restaurantService.GetAllRestaurantsAsCardsAsync();
            Assert.That(r.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteRestaurantsByBrandIdResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            await restaurantService.DeleteRestaurantsByBrandIdAsync(brandId);
            var r = await restaurantService.GetAllRestaurantsAsCardsAsync();
            Assert.That(r.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task EditRestaurantByIdSuccessfulResult()
        {
            string rId = "3a77e8ec-2d43-4eff-9f3a-822a2d7b8be3";
            var r = await restaurantService.GetRestaurantForEditByIdAsync(rId);
            r.Name = "new test name";
            var res=await restaurantService.EditRestaurantByIdAsync(rId, r);
            Assert.That(res.IsRestaurantAdded, Is.EqualTo(true));
        }
        [Test]
        public async Task EditRestaurantByIdInvalidSettlemntOfAddressResult()
        {
            string rId = "3a77e8ec-2d43-4eff-9f3a-822a2d7b8be3";
            var r = await restaurantService.GetRestaurantForEditByIdAsync(rId);
            r.SettlementId = 2;
            var res=await restaurantService.EditRestaurantByIdAsync(rId, r);        
            Assert.That(res.IsRestaurantAdded, Is.EqualTo(false));
        }
        [Test]
        public async Task EditRestaurantByIdInvalidAddressResult()
        {
            string rId = "3a77e8ec-2d43-4eff-9f3a-822a2d7b8be3";
            var r = await restaurantService.GetRestaurantForEditByIdAsync(rId);
            r.Address = "ne pozna";
            var res = await restaurantService.EditRestaurantByIdAsync(rId, r);
            Assert.That(res.IsRestaurantAdded, Is.EqualTo(false));
        }

        [Test]
        public async Task ExistsByIdTrueResult()
        {
            string rId = "3a77e8ec-2d43-4eff-9f3a-822a2d7b8be3";
            bool res = await restaurantService.ExistsByIdAsync(rId);
            Assert.That(res, Is.EqualTo(true));
        }
        [Test]
        public async Task ExistsByIdFalseResult()
        {
            string rId = "3a78e8ec-2d43-4eff-9f3a-822a2d7b8be3";
            bool res = await restaurantService.ExistsByIdAsync(rId);
            Assert.That(res, Is.EqualTo(false));
        }

        [Test]
        public async Task GetAllRestaurantsAsCardsResult()
        {
            var r = await restaurantService.GetAllRestaurantsAsCardsAsync();
            Assert.That(r.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetOwnerRestaurantsByOnwerIdResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            var r = await restaurantService.GetOwnerRestaurantsByOnwerIdAsync(ownerId);
            Assert.That(r.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetRestaurantDetailsByIdResult()
        {
            string rId = "3a77e8ec-2d43-4eff-9f3a-822a2d7b8be3";
            var r = await restaurantService.GetRestaurantDetailsByIdAsync(rId);
            Assert.That(r.Name, Is.EqualTo("При Гошо на бургер"));
            Assert.That(r.PhoneNumber, Is.EqualTo("+35912345678"));
            Assert.That(r.Address, Is.EqualTo("Сан Стефано 62"));
            Assert.That(r.Settlement, Is.EqualTo("гр.Бургас"));
            Assert.That(r.Region, Is.EqualTo("обл.Бургас"));
            Assert.That(r.Brand, Is.EqualTo("Gosho brand"));
            Assert.That(r.Category, Is.EqualTo("Ресторант"));
            Assert.That(r.ImageUrl, Is.EqualTo("https://www.bfu.bg/uploads/sliders/slide_12.jpg"));
            Assert.That(r.Latitude, Is.EqualTo(42.5029812M));
            Assert.That(r.Longitude, Is.EqualTo(27.4701599M));
        }

        [Test]
        public async Task GetRestaurantForEditByIdResult()
        {
            string rId = "3a77e8ec-2d43-4eff-9f3a-822a2d7b8be3";
            var r = await restaurantService.GetRestaurantForEditByIdAsync(rId);
            Assert.That(r.Name, Is.EqualTo("При Гошо на бургер"));
            Assert.That(r.PhoneNumber, Is.EqualTo("+35912345678"));
            Assert.That(r.Address, Is.EqualTo("Сан Стефано 62"));
            Assert.That(r.SettlementId, Is.EqualTo(1));
            Assert.That(r.CategoryId, Is.EqualTo(1));
            Assert.That(r.ImageUrl, Is.EqualTo("https://www.bfu.bg/uploads/sliders/slide_12.jpg"));
        }

        [Test]
        public async Task GetRestaurantsByBrandIdResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            var r = await restaurantService.GetRestaurantsByBrandIdAsync(brandId);
            Assert.That(r.Count(), Is.EqualTo(2));          
        }

        [Test]
        public async Task RestaurantOwnedByOwnerByRestaurantIdAndOwnerIdTrueResult()
        {
            string rId = "3a77e8ec-2d43-4eff-9f3a-822a2d7b8be3";
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            bool res = await restaurantService.RestaurantOwnedByOwnerByRestaurantIdAndOwnerId(rId, ownerId);
            Assert.That(res, Is.EqualTo(true));
        }
        [Test]
        public async Task RestaurantOwnedByOwnerByRestaurantIdAndOwnerIdFalseResult()
        {
            string rId = "3a77e8ec-2d43-4eff-9f3a-822a2d7b8be3";
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822c";
            bool res = await restaurantService.RestaurantOwnedByOwnerByRestaurantIdAndOwnerId(rId, ownerId);
            Assert.That(res, Is.EqualTo(false));
        }
    }
}
