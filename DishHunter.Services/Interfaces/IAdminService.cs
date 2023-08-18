namespace DishHunter.Services.Data.Interfaces
{
    using System.Threading.Tasks;
    using Models.Admin;

    public interface IAdminService
    {
        Task<bool> IsUserAdminByUserId(string userId);
        Task MakeUserAdminAsyncByUserId(string userId);
        Task<IEnumerable<UserListTransferModel>> GetAllUsersAsync();
        Task DeleteUserByIdAsync(string userId);
        Task<bool> UserExistsByIdAsync(string userId);
    }
}
