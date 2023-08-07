namespace DishHunter.Services.Data.Interfaces
{
    using Models.Category;
    public interface ICategoryService
    {
        Task<IEnumerable<CategorySelectTransferModel>> AllCategoriesAsync();
        Task<int?> CategoryExistsByNameAsync(string categoryName);
        Task<int> CreateCategoryAsync(CategoryPostTransferModel category);
        Task<CategoryPostTransferModel> GetCategoryForEditByIdAsync(int categoryId);
        Task EditCategoryByIdAsync(int categoryId, CategoryPostTransferModel category);
        Task DeleteCategoryByIdAsync(int categoryId);
    }
}
