namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using DishHunter.Data;
    using Models.Settlement;
    using Interfaces;
    using DishHunter.Data.Models.Restaurant;

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

        public async Task<int?> SettlementExistsByNameAndRegionAsync(string name, string region)
        {
            Settlement? settlement = await dbContext
                .Settlements
                .Where(s => s.IsActive)
                .FirstOrDefaultAsync(s => s.SettlementName == name && s.Region == s.Region);
            if (settlement == null)
                return null;
            return settlement.Id;
        }
    }
}
