
namespace DishHunter.Data.Models.Restaurant
{
    using System.ComponentModel.DataAnnotations;
    public class Settlement
    {
        public Settlement()
        {
            Restaurants = new HashSet<Restaurant>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string SettlementName { get; set; } = null!;
        [Required]
        public string Region { get; set; } = null!;

		public bool IsActive { get; set; }

		public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
