namespace DishHunter.Web.ViewModels.Menu
{
    using Brand;

    public class MenuExcelFormViewModel
    {
        public MenuExcelFormViewModel()
        {
            Brands = new HashSet<BrandSelectViewModel>();
        }
        public string BrandId { get; set; } = null!;
        public IEnumerable<BrandSelectViewModel> Brands { get; set; }
    }
}
