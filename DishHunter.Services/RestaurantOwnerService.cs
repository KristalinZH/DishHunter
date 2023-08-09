namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using DishHunter.Data;
    using DishHunter.Data.Models.Account;
    using Models.RestaurantOwner;
    using Interfaces;

    public class RestaurantOwnerService : IRestaurantOwnerService
    {
        private readonly ApplicationDbContext dbContext;
        public RestaurantOwnerService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task CreateOwnerByUserId(string userId, RestaurantOwnerPostTransferModel owner)
        {
            ApplicationUser user = await dbContext.Users
                .FirstAsync(u => u.Id.ToString() == userId);
            user.PhoneNumber = owner.PhoneNumber;
            RestaurantOwner ownerToAdd = new RestaurantOwner()
            {
                UserId = Guid.Parse(userId)
            };
            await dbContext.RestaurantOwners.AddAsync(ownerToAdd);
            await dbContext.SaveChangesAsync();
        }

        public async Task<string?> GetOwnerIdByUserId(string userId)
        {
            RestaurantOwner? owner = await dbContext.RestaurantOwners
                .Where(ro=>ro.IsActive)
                .FirstOrDefaultAsync(ro=>ro.UserId!=null && ro.UserId.ToString()==userId);
            if (owner == null)
                return null;
            return owner.Id.ToString();
        }

        public async Task<bool> OwnerExistsByPhoneNumberAsync(string phoneNumber)
            => await dbContext.RestaurantOwners
                .Include(ro => ro.User)
                .AnyAsync(ro => ro.User != null && ro.User.PhoneNumber == phoneNumber);

        public async Task<bool> OwnerExistsByUserIdAsync(string userId)
            => await dbContext.RestaurantOwners
                .AnyAsync(ro => ro.UserId!=null && ro.UserId.ToString() == userId);
    }
}
