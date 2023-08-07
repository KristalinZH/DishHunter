namespace DishHunter.Services.Data.Models.Brand
{
    public class BrandListTransferModel
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
    }
}
