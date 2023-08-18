namespace DishHunter.Services.Data
{
    using DishHunter.Data;
    using DishHunter.Services.Data.Interfaces;
    using DishHunter.Services.Data.Models.Admin;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using static Common.RolesConstants;
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext dbContext;
        public AdminService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task DeleteUserByIdAsync(string userId)
        {
            var user = await dbContext.Users.FirstAsync(u => u.Id.ToString() == userId);
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<UserListTransferModel>> GetAllUsersAsync()
        { 
            var users=await dbContext.Users
                .OrderBy(u=>u.UserName)
                .ToArrayAsync();
            var model = users.Select(u => new UserListTransferModel()
            {
                Id = u.Id.ToString(),
                UserName = u.UserName
            }); 
            foreach(var u in model)
            {
                u.UserType = await GetUserType(u.Id);
            }
            return model;
        }
        public async Task<bool> IsUserAdminByUserId(string userId)
        {
            var adminRoleId = await dbContext.Roles.FirstAsync(r => r.Name == AdminRoleName);
            return await dbContext.UserRoles
                .AnyAsync(ur => ur.UserId.ToString() == userId && ur.RoleId == adminRoleId.Id);
        }
        public async Task MakeUserAdminAsyncByUserId(string userId)
        {
            IdentityRole<Guid> adminRoleId = await dbContext.Roles.FirstAsync(r => r.Name == AdminRoleName);
            IdentityUserRole<Guid> userRole = await dbContext.UserRoles.FirstAsync(ur => ur.UserId.ToString() == userId);
            Guid uid = userRole.UserId;
            dbContext.UserRoles.Remove(userRole);
            await dbContext.UserRoles.AddAsync(new IdentityUserRole<Guid>()
            {
                UserId = uid,
                RoleId = adminRoleId.Id
            });
            await dbContext.SaveChangesAsync();
        }
        public async Task<bool> UserExistsByIdAsync(string userId)
            => await dbContext.Users.AnyAsync(u => u.Id.ToString() == userId);
        private async Task<string> GetUserType(string userId)
        {
            string userType = "Потребител";
            bool isAdmin = await IsUserAdminByUserId(userId);
            if (isAdmin)
            {
                userType = "Администратор";
                return userType;
            }
            var user = await dbContext.Users.FirstAsync(u => u.Id.ToString() == userId);
            if(user.RestaurantOwnerId.HasValue)
            {
                userType = "Ресторантьор";
                return userType;
            }
            return userType;
        }
    }
}
