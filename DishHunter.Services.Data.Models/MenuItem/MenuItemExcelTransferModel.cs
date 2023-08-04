namespace DishHunter.Services.Data.Models.MenuItem
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.MenuItem;
    using static Common.ValidationErrorMessages;
    public class MenuItemExcelTransferModel
    {
        [Required]
        [StringLength(FoodCategoryMaxLenght, MinimumLength = FoodCategoryMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string FoodCategory { get; set; } = null!;
        [Required]
        [StringLength(NameMaxLenght, MinimumLength = NameMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string Name { get; set; } = null!;
        [Range(typeof(decimal),PriceMinValue,PriceMaxValue,ErrorMessage = PriceValueMessage)]
        public decimal Price { get; set; }
        [Required]
        [StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string Description { get; set; } = null!;
        [Required]
        [MaxLength(UrlMaxLenght, ErrorMessage = UrlLenghtMessage)]
        public string ImageUrl { get; set; } = null!;
    }
}
