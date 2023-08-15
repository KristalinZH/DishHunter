namespace DishHunter.Web.ViewModels.Brand
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Brand;
    using static Common.ValidationErrorMessages;
    public class BrandFormViewModel
    {
        [Required]
        [StringLength(BrandNameMaxLenght, MinimumLength = BrandNameMinLenght, ErrorMessage = FieldLenghtMessage)]
        [Display(Name ="Име")]
        public string BrandName { get; set; } = null!;
        [Required]
        [MaxLength(UrlMaxLenght, ErrorMessage = UrlLenghtMessage)]
		[Display(Name = "Линк към лого")]
		public string LogoUrl { get; set; } = null!;
        [Required]
        [MaxLength(UrlMaxLenght, ErrorMessage = UrlLenghtMessage)]
		[Display(Name = "Линк към уебсайт")]
		public string WebsiteUrl { get; set; } = null!;
        [Required]
        [StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLenght, ErrorMessage = FieldLenghtMessage)]
		[Display(Name = "Описание")]
		public string Description { get; set; } = null!;
    }
}
