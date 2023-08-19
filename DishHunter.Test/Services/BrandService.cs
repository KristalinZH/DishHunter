namespace DishHunter.Test.Services
{
    using BaseTest;
    using DishHunter.Services.Data;
    using DishHunter.Services.Data.Models.Brand;
    using Microsoft.EntityFrameworkCore;

    [TestFixture]
    internal class BrandService : BaseTestClass
    {
        [Test]
        public async Task CreateBrandAsync_ShouldReturnBrandId()
        {
            var brandModel = new BrandPostTransferModel
            {
                BrandName = "Test Brand",
                LogoUrl = "test_logo_url",
                WebsiteUrl = "test_website_url",
                Description = "Test brand description"
            };

            string brandId = await brandService.CreateBrandAsync("2618f422-fb08-42a7-bd9c-ffc4d311822a", brandModel);

            Assert.That(brandId, Is.Not.EqualTo(null));
        }
        [Test]
        public async Task BrandExistResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            bool isExistring = await brandService.ExistsByIdAsync(brandId);
            Assert.That(isExistring, Is.EqualTo(true));
        }
        [Test]
        public async Task BrandDontExistResult()
        {
            string brandId = "14d93c12-9a8c-40ce-a6ae-b4d7c980d708";
            bool isExistring = await brandService.ExistsByIdAsync(brandId);
            Assert.That(isExistring, Is.EqualTo(false));
        }
        [Test]
        public async Task GetOwnerId()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            string result = await brandService.GetBrandOwnerIdAsync(brandId);
            Assert.That(result, Is.EqualTo(ownerId));
        }

        [Test]
        public async Task GetAllBrandsResult()
        {
            var query = new BrandQueryTransferModel()
            {
                SearchString = null
            };
            query = await brandService.GetAllBrandsAsCardsAsync(query);
            Assert.That(query.Brands.Count(), Is.EqualTo(5));
        }
        [Test]
        public async Task GetTop3BrandsResult()
        {
            var brands = await brandService.GetTop3BrandsAsCardsAsync();
            Assert.That(brands.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetBrandForEditResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            var result = await brandService.GetBrandForEditByIdAsync(brandId);
            Assert.That(result.BrandName, Is.EqualTo("Gosho brand"));
            Assert.That(result.Description, Is.EqualTo("Nai gotinata veriga za burzo hranene v burgas"));
            Assert.That(result.LogoUrl, Is.EqualTo("https://lh5.googleusercontent.com/p/AF1QipNY3AIc5MD-tXpqXou00G2HougLrCEBUolx0Y8P=w260-h175-n-k-no"));
            Assert.That(result.WebsiteUrl, Is.EqualTo("https://mcdonalds.bg/en/"));
        }
        [Test]
        public async Task EditBrandResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            var result = await brandService.GetBrandForEditByIdAsync(brandId);
            result.Description = "Nai velikata veriga na George Parnarev";
            await brandService.EditBrandByIdAsync(brandId, result);
            result = await brandService.GetBrandForEditByIdAsync(brandId);
            Assert.That(result.Description, Is.EqualTo("Nai velikata veriga na George Parnarev"));
        }
        [Test]
        public async Task DeleteBrandResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            await brandService.DeleteBrandByIdAsync(brandId);
            bool result = await brandService.ExistsByIdAsync(brandId);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task GetOwnersBrandResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            var brands = await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId);
            Assert.That(brands.Count(), Is.EqualTo(4));
        }

        [Test]
        public async Task GetBrandDetailsResult()
        {
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d707";
            var result = await brandService.GetBrandDetailsByIdAsync(brandId);

            Assert.That(result.BrandName, Is.EqualTo("Gosho brand"));
            Assert.That(result.Description, Is.EqualTo("Nai gotinata veriga za burzo hranene v burgas"));
            Assert.That(result.LogoUrl, Is.EqualTo("https://lh5.googleusercontent.com/p/AF1QipNY3AIc5MD-tXpqXou00G2HougLrCEBUolx0Y8P=w260-h175-n-k-no"));
            Assert.That(result.WebsiteUrl, Is.EqualTo("https://mcdonalds.bg/en/"));
            Assert.That(result.Menus.Count(), Is.EqualTo(2));
            Assert.That(result.Restaurants.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetBrandsForSelectResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            var brands = await brandService.GetBrandsForSelectByOwnerId(ownerId);
            Assert.That(brands.Count(), Is.EqualTo(4));
        }
        [Test]
        public async Task BrandOwnedByOwnerIdAndBrandIdTrueResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d708";
            bool result = await brandService.BrandOwnedByOwnerIdAndBrandIdAsync(brandId, ownerId);
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task BrandOwnedByOwnerIdAndBrandIdFalseResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            string brandId = "15d93c12-9a8c-40ce-a6ae-b4d7c980d711";
            bool result = await brandService.BrandOwnedByOwnerIdAndBrandIdAsync(brandId, ownerId);
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public async Task AnyBrandOwnedByOwnerByOwnerIdTrueResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            bool result = await brandService.AnyBrandOwnedByOwnerByOwnerIdAsync(ownerId);
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task AnyBrandOwnedByOwnerByOwnerIdFalseResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822c";
            bool result = await brandService.AnyBrandOwnedByOwnerByOwnerIdAsync(ownerId);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task DeleteBrandsByOwnerIdResult()
        {
            string ownerId = "2618f422-fb08-42a7-bd9c-ffc4d311822a";
            await brandService.DeleteBrandsByOwnerIdAsync(ownerId);
            var query = new BrandQueryTransferModel()
            {
                SearchString = null
            };
            query = await brandService.GetAllBrandsAsCardsAsync(query);
            Assert.That(query.Brands.Count(), Is.EqualTo(1));
        }
    }
}
