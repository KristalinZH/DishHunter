namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using DishHunter.Data;
    using DishHunter.Data.Models.Restaurant;
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

        public async Task<int?> SettlementExistsByNameAndRegionAsync(string name, string region)
        {
            string nameWildcard = $"%{name}";
            string regionWildCard = $"%{region}";
            Settlement? settlement = await dbContext
                .Settlements
                .Where(s => s.IsActive)
                .FirstOrDefaultAsync(s => s.SettlementName == name && s.Region == region);
            if (settlement == null)
                return null;
            return settlement.Id;
        }

        public async Task<GeoSettlementTransferModel> GeoSettlementInfoByIdAsync(int settlementId)
        {
            Settlement settlement = await dbContext.Settlements
                .Where(s => s.IsActive)
                .FirstAsync(s => s.Id == settlementId);
            return new GeoSettlementTransferModel()
            {
                SettlementName = settlement.SettlementName.Split('.')[1],
                Region = settlement.Region.Split('.')[1]
            };
        }

        public async Task CreateSettlementAsync(SettlementPostTransferModel settlement)
        {
            Settlement settlementToAdd = new Settlement()
            {
                SettlementName = settlement.SettlementName,
                Region = settlement.Region
            };
            await dbContext.Settlements.AddAsync(settlementToAdd);
            await dbContext.SaveChangesAsync();
        }

        public async Task<SettlementPostTransferModel> GetSettlementForEditByIdAsync(int settlementId)
        {
            Settlement settlement = await dbContext.Settlements
                .Where(s => s.IsActive)
                .FirstAsync(s => s.Id == settlementId);
            return new SettlementPostTransferModel()
            {
                SettlementName = settlement.SettlementName,
                Region = settlement.Region
            };
        }

        public async Task EditSettlementByIdAsync(int settlementId, SettlementPostTransferModel settlement)
        {
            Settlement settlementToAdd = await dbContext.Settlements
                 .Where(s => s.IsActive)
                 .FirstAsync(s => s.Id == settlementId);
            settlementToAdd.SettlementName = settlement.SettlementName;
            settlementToAdd.Region = settlement.Region;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteSettlementByIdAsync(int settlementId)
        {
            Settlement settlementToDelete = await dbContext.Settlements
                 .Where(s => s.IsActive)
                 .FirstAsync(s => s.Id == settlementId);
            settlementToDelete.IsActive = false;
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int settlementId)
            => await dbContext.Settlements
            .Where(s => s.IsActive)
            .AnyAsync(s => s.Id == settlementId);
    }
}
