namespace DishHunter.Services.Data.Models.Brand
{
    public class BrandQueryTransferModel
    {
        public BrandQueryTransferModel()
        {
            Brands = new HashSet<BrandsCardTransferModel>();
        }
        public string? SearchString { get; set; }
        public IEnumerable<BrandsCardTransferModel> Brands { get; set; }
    }
}
