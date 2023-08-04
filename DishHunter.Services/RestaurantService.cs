namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using DishHunter.Data;
    using DishHunter.Data.Models.Restaurant;
    using Models.Restaurant;
    using Interfaces;
    using System.Collections.Generic;

    public class RestaurantService : IRestaurantService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ISettlementService settlementService;
        public RestaurantService(ApplicationDbContext _dbContext, ISettlementService _settlementService)
        {
            dbContext = _dbContext;
            settlementService = _settlementService;
        }

        public Task AddRestaurantsByBrandIdAsync(IEnumerable<RestaurantExcelTransferModel> restaurants, string brandId)
        {
            throw new NotImplementedException();
        }
    }
}
