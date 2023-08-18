namespace DishHunter.Test.Services
{
    using BaseTest;
    using DishHunter.Services.Data.Models.Category;

    internal class CategoryService : BaseTestClass
    {
        [Test]
        public async Task AllCategoriesResult()
        {
            var result = await categoryService.AllCategoriesAsync();
            Assert.That(result.Count(), Is.EqualTo(2));
        }
        [Test]
        public async Task CategoryExistsByNameExistResult()
        {
            string name = "Ресторант";
            var result = await categoryService.CategoryExistsByNameAsync(name);
            Assert.That(result, Is.EqualTo(1));
        }
        [Test]
        public async Task CategoryExistsByNameNotExistResult()
        {
            string name = "ne pozna";
            var result = await categoryService.CategoryExistsByNameAsync(name);
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public async Task CreateCategoryResult()
        {
            CategoryPostTransferModel c = new CategoryPostTransferModel()
            {
                CategoryName = "test12345"
            };
            await categoryService.CreateCategoryAsync(c);          
            var result = await categoryService.AllCategoriesAsync();
            Assert.That(result.Count(), Is.EqualTo(2));
        }
        [Test]
        public async Task DeleteCategoryByIdResult()
        {
            int id = 1;
            await categoryService.DeleteCategoryByIdAsync(id);
            var result = await categoryService.AllCategoriesAsync();
            Assert.That(result.Count(), Is.EqualTo(1));
        }
        [Test]
        public async Task EditCategoryByIdResult()
        {
            int id = 1;
            CategoryPostTransferModel c = new CategoryPostTransferModel()
            {
                CategoryName = "promqna"
            };
            await categoryService.EditCategoryByIdAsync(id, c);
            var result = (await categoryService.AllCategoriesAsync()).ToList();
            Assert.That(result[0].CategoryName, Is.EqualTo("promqna"));
        }
        [Test]
        public async Task ExistsByIdTrueResult()
        {
            int id = 1;
            bool result = await categoryService.ExistsByIdAsync(id);
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task ExistsByIdFalseResult()
        {
            int id = 5;
            bool result = await categoryService.ExistsByIdAsync(id);
            Assert.That(result, Is.EqualTo(false));
        }
    }
}
