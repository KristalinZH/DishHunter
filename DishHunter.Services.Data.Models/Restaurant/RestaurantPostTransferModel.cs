namespace DishHunter.Services.Data.Models.Restaurant
{
    using DishHunter.Services.Data.Models.Category;
    using DishHunter.Services.Data.Models.Settlement;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Common.EntityValidationConstants.Restaurant;
    using static Common.ValidationErrorMessages;
    public class RestaurantPostTransferModel
    {
        public RestaurantPostTransferModel()
        {
            Settlements = new HashSet<SettlementSelectTransferModel>();
            Categories = new HashSet<CategorySelectTransferModel>();
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
        public int CategoryId { get; set; }
        public IEnumerable<CategorySelectTransferModel> Categories { get; set; }
        public int SettlementId { get; set; }
        public IEnumerable<SettlementSelectTransferModel> Settlements { get; set; }
    }
}
