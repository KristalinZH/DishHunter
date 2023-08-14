namespace DishHunter.Web.ViewModels.MenuItem
{
    using System.ComponentModel.DataAnnotations;
    using Menu;
    using static Common.EntityValidationConstants.MenuItem;
    using static Common.ValidationErrorMessages;
    public class MenuItemFormViewModel
    {
        public MenuItemFormViewModel()
        {
            Menus = new HashSet<MenuSelectViewModel>();
        }
        [Required]
        [StringLength(FoodCategoryMaxLenght, MinimumLength = FoodCategoryMinLenght, ErrorMessage = FieldLenghtMessage)]
        [Display(Name="Тип")]
        public string FoodCategory { get; set; } = null!;
        [Required]
        [StringLength(NameMaxLenght, MinimumLength = NameMinLenght, ErrorMessage = FieldLenghtMessage)]
		[Display(Name = "Име")]
		public string Name { get; set; } = null!;
        [Range(typeof(decimal), PriceMinValue, PriceMaxValue, ErrorMessage = PriceValueMessage)]
		[Display(Name = "Цена")]
		public decimal Price { get; set; }
        [Required]
        [StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLenght, ErrorMessage = FieldLenghtMessage)]
		[Display(Name = "Описание")]
		public string Description { get; set; } = null!;
        [Required]
        [MaxLength(UrlMaxLenght, ErrorMessage = UrlLenghtMessage)]
		[Display(Name = "Линк към снимка")]
		public string ImageUrl { get; set; } = null!;
        public int MenuId { get; set; }
        public IEnumerable<MenuSelectViewModel> Menus { get; set; }
    }
}
