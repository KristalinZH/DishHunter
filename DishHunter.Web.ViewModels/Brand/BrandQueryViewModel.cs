namespace DishHunter.Web.ViewModels.Brand
{
    public class BrandQueryViewModel
    {
        public BrandQueryViewModel()
        {
            Brands = new HashSet<BrandsCardViewModel>();
        }
        public string? SearchString { get; set; }
        public IEnumerable<BrandsCardViewModel> Brands { get; set; }
    }
}
