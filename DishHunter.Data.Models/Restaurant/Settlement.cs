
namespace DishHunter.Data.Models.Restaurant
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Settlement;
    public class Settlement
    {
        public Settlement()
        {
            Restaurants = new HashSet<Restaurant>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(SettlementNameMaxLenght)]
        public string SettlementName { get; set; } = null!;
        [Required]
        [MaxLength(RegionMaxLenght)]
        public string Region { get; set; } = null!;
		public bool IsActive { get; set; }
		public virtual IEnumerable<Restaurant> Restaurants { get; set; }
    }
}
