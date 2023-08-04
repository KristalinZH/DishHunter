namespace DishHunter.Services.Data.Models.Brand
{
    using System.ComponentModel.DataAnnotations;
    using Menu;
    using Restaurant;
    using static Common.EntityValidationConstants.Brand;
    using static Common.ValidationErrorMessages;
    public class BrandPostTransferModel
    {
        [Required]
        [StringLength(BrandNameMaxLenght, MinimumLength = BrandNameMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string BrandName { get; set; } = null!;
        [Required]
        [MaxLength(UrlMaxLenght, ErrorMessage = UrlLenghtMessage)]
        public string LogoUrl { get; set; } = null!;
        [Required]
        [MaxLength(UrlMaxLenght, ErrorMessage = UrlLenghtMessage)]
        public string WebsiteUrl { get; set; } = null!;
        [Required]
        [StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string Description { get; set; } = null!;
    }
}
