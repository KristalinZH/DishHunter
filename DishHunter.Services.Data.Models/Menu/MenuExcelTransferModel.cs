namespace DishHunter.Services.Data.Models.Menu
{
    using DishHunter.Services.Data.Models.MenuItem;
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Menu;
    using static Common.ValidationErrorMessages;
    public class MenuExcelTransferModel
    {
        public MenuExcelTransferModel()
        {
            MenuItems = new HashSet<MenuItemExcelTransferModel>();
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
        public IEnumerable<MenuItemExcelTransferModel> MenuItems { get; set; }
    }
}
