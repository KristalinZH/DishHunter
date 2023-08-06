namespace DishHunter.Services.Data.Interfaces
{
    using Models.Geocoding;

    public interface IGeocodingService
    {
        Task<GeocodingStatusModel> RetreiveCoordinatesByAddressAndSettlementAsync(string address, string settlementName, string region);
    }
}
