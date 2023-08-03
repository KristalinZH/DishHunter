namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using DishHunter.Data;
    using DishHunter.Data.Models.Restaurant;
    using Models.Menu;
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
