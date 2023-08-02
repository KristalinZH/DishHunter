namespace DishHunter.Services.Data
{
    using DishHunter.Data;
    using Interfaces;

    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMenuItemService menuItemService;
        public MenuService(ApplicationDbContext _dbContext, IMenuItemService _menuItemService)
        {
            dbContext = _dbContext;
            menuItemService = _menuItemService;
        }
    }
}
