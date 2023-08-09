namespace DishHunter.Services.Data.Interfaces
{
    using Models.RestaurantOwner;

    public interface IRestaurantOwnerService
    {
        Task<bool> OwnerExistsByUserIdAsync(string userId);
        Task<bool> OwnerExistsByPhoneNumberAsync(string phoneNumber);
        Task CreateOwnerByUserId(string userId, RestaurantOwnerPostTransferModel owner);
        Task<string?> GetOwnerIdByUserId(string userId);
    }
}
