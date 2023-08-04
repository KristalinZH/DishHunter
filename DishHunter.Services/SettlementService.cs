namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using DishHunter.Data;
    using Models.Settlement;
    using Interfaces;

    public class SettlementService : ISettlementService
    {
        private readonly ApplicationDbContext dbContext;
        public SettlementService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<IEnumerable<SettlementSelectTransferModel>> AllSettlementsAsync()
            => await dbContext.Settlements
                    .Where(s => s.IsActive)
                    .Select(s => new SettlementSelectTransferModel()
                    {
                        Id = s.Id,
                        SettlementName = s.SettlementName,
                        Region = s.Region
                    }).ToArrayAsync();

        public async Task<bool> SettlementExistsByNameAndRegionAsync(string name, string region)
        {
            return true;
        }
    }
}
