
namespace DishHunter.Data.Models.Restaurant
{
    using System.ComponentModel.DataAnnotations;
    public class Category
    {
        public Category()
        {
            Restaurants = new HashSet<Restaurant>();
        }
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; } = null!;

		public bool IsActive { get; set; }
		public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
