namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using DishHunter.Data;
    using DishHunter.Data.Models.Restaurant;
    using Models.Category;
    using Interfaces;

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<IEnumerable<CategorySelectTransferModel>> AllCategoriesAsync()
            => await dbContext.Categories
                    .Where(c => c.IsActive)
                    .Select(c => new CategorySelectTransferModel()
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName
                    }).ToArrayAsync();

        public async Task<int?> CategoryExistsByNameAsync(string categoryName)
        {
            Category? category = await dbContext
                .Categories
                .FirstOrDefaultAsync(c => c.CategoryName == categoryName);
            if (category == null)
                return null;
            return category.Id;
        }

        public async Task CreateCategoryAsync(CategoryPostTransferModel category)
        {
            Category categoryToAdd = new Category()
            {
                CategoryName = category.CategoryName
            };
            await dbContext.Categories.AddAsync(categoryToAdd);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryByIdAsync(int categoryId)
        {
            Category category = await dbContext.Categories
                .Where(c => c.IsActive)
                .FirstAsync(c => c.Id == categoryId);
            category.IsActive = false;
            await dbContext.SaveChangesAsync();
        }

        public async Task EditCategoryByIdAsync(int categoryId, CategoryPostTransferModel category)
        {
            Category categoryForEdit = await dbContext.Categories
                .Where(c => c.IsActive)
                .FirstAsync(c => c.Id == categoryId);
            categoryForEdit.CategoryName = category.CategoryName;
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int categoryId)
            => await dbContext.Categories
            .Where(c => c.IsActive)
            .AnyAsync(c => c.Id == categoryId);

        public async Task<CategoryPostTransferModel> GetCategoryForEditByIdAsync(int categoryId)
        {
            Category category = await dbContext.Categories
                .Where(c => c.IsActive)
                .FirstAsync(c => c.Id == categoryId);
            return new CategoryPostTransferModel()
            {
                CategoryName = category.CategoryName
            };
        }
    }
}
