namespace DishHunter.Web.ViewModels.Restaurant
{
    using Brand;

    public class RestaurantExcelFormViewModel
    {
        public RestaurantExcelFormViewModel()
        {
            Brands = new HashSet<BrandSelectViewModel>();
        }
        public string BrandId { get; set; } = null!;
        public IEnumerable<BrandSelectViewModel> Brands { get; set; }
    }
}
