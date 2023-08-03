namespace DishHunter.Services.Data.Interfaces
{
    using Models.Settlement;

    public interface ISettlementService
    {
        Task<IEnumerable<SettlementSelectTransferModel>> AllSettlementsAsync();
    }
}
