namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Models.Geocoding;
    using Interfaces;
    using static Common.NotificationMessagesConstants;

    public class GeocodingService : IGeocodingService
    {
        private readonly HttpClient httpClient;
        public GeocodingService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }
        public async Task<GeocodingStatusModel> RetreiveCoordinatesByAddressAndSettlementAsync(string address, string settlementName, string region)
        {
            GeocodingStatusModel result = new GeocodingStatusModel()
            {
                AreCoordinatedFound = false,
                Message = NonExistringAddress,
                Latitude = null, 
                Longitude = null 
            };
            var query = $"{address}, {settlementName}, {region}";
            httpClient.DefaultRequestHeaders.Add("User-Agent", "DishHunter");
            var response = await httpClient.GetAsync($"https://nominatim.openstreetmap.org/search?q={query}&format=json");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var coordinates = JsonConvert.DeserializeAnonymousType(json, new[]
                {
                    new
                    {
                         Lat = "",
                         Lon = ""
                    }
                });
                if (coordinates == null)
                    return result;
                if (coordinates.Length > 1)
                {
                    result.Message = MultipleAddressesFound;
                    return result;
                }
                result.AreCoordinatedFound = true;
                result.Message = string.Empty;
                result.Latitude = decimal.Parse(coordinates[0].Lat);
                result.Longitude = decimal.Parse(coordinates[0].Lon);
            }
            return result;
        }
    }
}
