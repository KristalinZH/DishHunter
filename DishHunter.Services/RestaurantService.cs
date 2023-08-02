namespace DishHunter.Services.Data
{
    using DishHunter.Data;
    using Interfaces;

    public class RestaurantService : IRestaurantService
    {
        private readonly ApplicationDbContext dbContext;
        public RestaurantService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
    }
}
