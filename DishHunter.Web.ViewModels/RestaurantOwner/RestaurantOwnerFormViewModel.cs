namespace DishHunter.Web.ViewModels.RestaurantOwner
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.RestaurantOwner;
    using static Common.ValidationErrorMessages;
    public class RestaurantOwnerFormViewModel
    {
        [Required]
        [StringLength(PhoneNumberMaxLenght, MinimumLength = PhoneNumberMinLenght, ErrorMessage = FieldLenghtMessage)]
        [Phone]
        [Display(Name ="Телефон")]
        public string PhoneNumber { get; set; } = null!;
    }
}
