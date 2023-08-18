namespace DishHunter.Web.ViewModels.Category
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Category;
    using static Common.ValidationErrorMessages;
    public class CategoryFormViewModel
    {
        [Required]
        [StringLength(CategoryNameMaxLenght, MinimumLength = CategoryNameMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string CategoryName { get; set; } = null!;
    }
}
