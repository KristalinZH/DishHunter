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
        [Display(Name="Име на ресторанта")]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(AddressMaxLenght, MinimumLength = AddressMinLenght, ErrorMessage = FieldLenghtMessage)]
        [Display(Name = "Адрес")]
        public string Address { get; set; } = null!;
        [Required]
        [Phone]
        [StringLength(PhoneMaxLenght, MinimumLength = PhoneMinLenght, ErrorMessage = FieldLenghtMessage)]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [MaxLength(UrlMaxLenght, ErrorMessage = UrlLenghtMessage)]
        [Display(Name = "Линк")]
        public string ImageUrl { get; set; } = null!;
        [Display(Name = "Верига")]
        public string BrandId { get; set; } = null!;
        public IEnumerable<BrandSelectViewModel> Brands { get; set; }
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        [Display(Name = "Населено място")]
        public int SettlementId { get; set; }
        public IEnumerable<SettlementSelectViewModel> Settlements { get; set; }
    }
}
