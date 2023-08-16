namespace DishHunter.Services.Data.Interfaces
{
    using Models.Settlement;

    public interface ISettlementService
    {
        Task<IEnumerable<SettlementSelectTransferModel>> AllSettlementsAsync();
        Task<int?> SettlementExistsByNameAndRegionAsync(string name, string region);
        Task<GeoSettlementTransferModel> GeoSettlementInfoByIdAsync(int settlementId);
        Task CreateSettlementAsync(SettlementPostTransferModel settlement);
        Task<SettlementPostTransferModel> GetSettlementForEditByIdAsync(int settlementId);
        Task EditSettlementByIdAsync(int settlementId, SettlementPostTransferModel settlement);
        Task DeleteSettlementByIdAsync(int settlementId);
        Task<bool> ExistsByIdAsync(int settlementId);
    }
}
