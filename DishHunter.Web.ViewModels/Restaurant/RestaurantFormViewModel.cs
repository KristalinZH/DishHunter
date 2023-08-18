namespace DishHunter.Web.ViewModels.Restaurant
{
    using System.ComponentModel.DataAnnotations;
    using Brand;
    using Category;
    using Settlement;
    using static Common.EntityValidationConstants.Restaurant;
    using static Common.ValidationErrorMessages;
    public class RestaurantFormViewModel
    {
        public RestaurantFormViewModel()
        {
            Brands = new HashSet<BrandSelectViewModel>();
            Categories = new HashSet<CategoryViewModel>();
            Settlements = new HashSet<SettlementSelectViewModel>();
        }
        [Required]
        [StringLength(NameMaxLenght, MinimumLength = NameMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(AddressMaxLenght, MinimumLength = AddressMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string Address { get; set; } = null!;
        [Required]
        [Phone]
        [StringLength(PhoneMaxLenght, MinimumLength = PhoneMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [MaxLength(UrlMaxLenght, ErrorMessage = UrlLenghtMessage)]
        public string ImageUrl { get; set; } = null!;
        public string BrandId { get; set; } = null!;
        public IEnumerable<BrandSelectViewModel> Brands { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public int SettlementId { get; set; }
        public IEnumerable<SettlementSelectViewModel> Settlements { get; set; }
    }
}
