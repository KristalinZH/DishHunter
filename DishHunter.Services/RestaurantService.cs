namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using DishHunter.Data;
    using DishHunter.Data.Models.Restaurant;
    using Models.Restaurant;
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
