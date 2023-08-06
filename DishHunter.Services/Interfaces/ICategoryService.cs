﻿namespace DishHunter.Services.Data.Interfaces
{
    using Models.Category;
    public interface ICategoryService
    {
        Task<IEnumerable<CategorySelectTransferModel>> AllCategoriesAsync();
        Task<int?> CategoryExistsByNameAsync(string categoryName);
    }
}
