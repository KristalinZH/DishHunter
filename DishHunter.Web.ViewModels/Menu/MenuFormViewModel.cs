namespace DishHunter.Web.ViewModels.Menu
{
	using System.ComponentModel.DataAnnotations;
	using Brand;
	using static Common.EntityValidationConstants.Menu;
	using static Common.ValidationErrorMessages;
	public class MenuFormViewModel
	{
		public MenuFormViewModel()
		{
			Brands = new HashSet<BrandSelectViewModel>();
		}
		[Required]
		[StringLength(MenuTypeMaxLenght, MinimumLength = MenuTypeMinLenght, ErrorMessage = FieldLenghtMessage)]
		public string MenuType { get; set; } = null!;
		[Required]
		[StringLength(FoodTypeMaxLenght, MinimumLength = FoodTypeMinLenght, ErrorMessage = FieldLenghtMessage)]
		public string FoodType { get; set; } = null!;
		[Required]
		[StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLenght, ErrorMessage = FieldLenghtMessage)]
		public string Description { get; set; } = null!;
		public string BrandId { get; set; } = null!;
		public IEnumerable<BrandSelectViewModel> Brands { get; set; }
	}
}
