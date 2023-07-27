
namespace DishHunter.Data.Models.Restaurant
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Category;
    public class Category
    {
        public Category()
        {
            Restaurants = new HashSet<Restaurant>();
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(CategoryNameMaxLenght)]
        public string CategoryName { get; set; } = null!;
		public bool IsActive { get; set; }
		public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
