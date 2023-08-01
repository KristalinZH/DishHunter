namespace DishHunter.Services.Data.Models.Brand
{
    using Menu;
    using Restaurant;

    public class DetailsBrandTransferModel
    {
        public DetailsBrandTransferModel()
        {
            Restarants = new HashSet<BrandRestaurantTranferModel>();
            Menus = new HashSet<BrandMenuTrasnferModel>();
        }
        public string BrandName { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        public string WebsiteUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IEnumerable<BrandRestaurantTranferModel> Restarants;
        public IEnumerable<BrandMenuTrasnferModel> Menus { get; set; }
    }
}
