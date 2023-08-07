namespace DishHunter.Services.Data.Models.Category
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Category;
    using static Common.ValidationErrorMessages;
    public class CategoryPostTransferModel
    {
        [Required]
        [StringLength(CategoryNameMaxLenght, MinimumLength = CategoryNameMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string CategoryName { get; set; } = null!;
    }
}
