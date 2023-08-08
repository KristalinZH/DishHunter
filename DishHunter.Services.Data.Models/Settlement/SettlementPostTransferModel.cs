namespace DishHunter.Services.Data.Models.Settlement
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Settlement;
    public class SettlementPostTransferModel
    {
        [Required]
        [MaxLength(SettlementNameMaxLenght)]
        public string SettlementName { get; set; } = null!;
        [Required]
        [MaxLength(RegionMaxLenght)]
        public string Region { get; set; } = null!;
    }
}
