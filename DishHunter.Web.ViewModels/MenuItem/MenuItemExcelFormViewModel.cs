namespace DishHunter.Web.ViewModels.MenuItem
{
	using Menu;

	public class MenuItemExcelFormViewModel
	{
        public MenuItemExcelFormViewModel()
        {
			Menus = new HashSet<MenuSelectViewModel>();
		}
        public int MenuId { get; set; }
		public IEnumerable<MenuSelectViewModel> Menus { get; set; }
	}
}
