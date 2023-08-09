namespace DishHunter.Services.Data.Models.RestaurantOwner
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.RestaurantOwner;
    using static Common.ValidationErrorMessages;
    public class RestaurantOwnerPostTransferModel
    {
        [Required]
        [StringLength(PhoneNumberMaxLenght, MinimumLength = PhoneNumberMinLenght, ErrorMessage = FieldLenghtMessage)]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
    }
}
