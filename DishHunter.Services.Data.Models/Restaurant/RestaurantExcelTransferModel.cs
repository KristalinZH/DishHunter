namespace DishHunter.Services.Data.Models.Restaurant
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Restaurant;
    using static Common.EntityValidationConstants.Category;
    using static Common.EntityValidationConstants.Settlement;
    using static Common.ValidationErrorMessages;
    public class RestaurantExcelTransferModel
    {
        [Required]
        [StringLength(NameMaxLenght, MinimumLength = NameMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(RegionMaxLenght, MinimumLength = RegionMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string Region { get; set; } = null!;
        [Required]
        [StringLength(SettlementNameMaxLenght, MinimumLength = SettlementNameMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string SettlementName { get; set; } = null!;
        [Required]
        [StringLength(AddressMaxLenght, MinimumLength = AddressMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string Address { get; set; } = null!;
        [Required]
        [Phone]
        [StringLength(PhoneMaxLenght, MinimumLength = PhoneMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [StringLength(CategoryNameMaxLenght,MinimumLength =CategoryNameMinLenght,ErrorMessage = FieldLenghtMessage)]
        public string CategoryName { get; set; } = null!;
        [Required]
        [MaxLength(UrlMaxLenght, ErrorMessage = UrlLenghtMessage)]
        public string ImageUrl { get; set; } = null!;
    }
}
