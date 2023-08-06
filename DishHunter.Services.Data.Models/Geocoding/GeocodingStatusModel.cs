namespace DishHunter.Services.Data.Models.Geocoding
{

    public class GeocodingStatusModel
    {
        public bool AreCoordinatedFound { get; set; }
        public string Message { get; set; } = null!;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
