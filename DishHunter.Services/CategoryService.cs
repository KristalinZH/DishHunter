namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using DishHunter.Data;
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
    }
}
