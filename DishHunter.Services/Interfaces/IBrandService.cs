namespace DishHunter.Services.Data.Interfaces
{
    using Models.Brand;

    public interface IBrandService
    {
        Task<string> CreateBrandAsync(string restaurantOwnerId, BrandPostTransferModel brandModel);
        Task<bool> ExistsByIdAsync(string brandId);
        Task<DetailsBrandTransferModel> GetBrandByIdAsync(string brandId);
        Task<string> GetBrandOwnerIdAsync(string brandId);
        Task<IEnumerable<AllBrandsTransferModel>> GetAllBrandsAsync();
        Task<BrandPostTransferModel> GetBrandForEditByIdAsync();
    }
}
