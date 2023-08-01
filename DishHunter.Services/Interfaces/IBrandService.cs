namespace DishHunter.Services.Data.Interfaces
{
    using Models.Brand;

    public interface IBrandService
    {
        Task<string> CreateBrandAsync(string restaurantOwnerId, AddBrandTransferModel brandModel);
        Task<DetailsBrandTransferModel> GetBrandByIdAsync(string brandId);
        Task<string> GetBrandOwnerId(string brandId);
    }
}
