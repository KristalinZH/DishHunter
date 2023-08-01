namespace DishHunter.Services.Data.Models.Brand
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Brand;
    public class AddBrandTransferModel
    {
        [Required]
        [StringLength(BrandNameMaxLenght, MinimumLength = BrandNameMinLenght)]
        public string BrandName { get; set; } = null!;
        [Required]
        [MaxLength(UrlMaxLenght)]
        public string LogoUrl { get; set; } = null!;
        [Required]
        [MaxLength(UrlMaxLenght)]
        public string WebsiteUrl { get; set; } = null!;
        [Required]
        [StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLenght)]
        public string Description { get; set; } = null!;
    }
}
