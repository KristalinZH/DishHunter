namespace DishHunter.Web.ViewModels.Settlement
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Settlement;
    using static Common.ValidationErrorMessages;
    public class SettlementFormViewModel
    {
        [Required]
        [StringLength(SettlementNameMaxLenght,MinimumLength =SettlementNameMinLenght,ErrorMessage =FieldLenghtMessage)]
        public string SettlementName { get; set; } = null!;
        [Required]
        [StringLength(RegionMaxLenght, MinimumLength = RegionMinLenght, ErrorMessage = FieldLenghtMessage)]
        public string Region { get; set; } = null!;
    }
}
