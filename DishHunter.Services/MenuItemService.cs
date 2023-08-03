namespace DishHunter.Services.Data
{
    using DishHunter.Data;
    using Interfaces;

    public class MenuItemService : IMenuItemService
    {
        private readonly ApplicationDbContext dbContext;
        public MenuItemService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
    }
}
