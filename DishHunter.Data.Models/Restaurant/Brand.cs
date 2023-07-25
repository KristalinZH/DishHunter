
namespace DishHunter.Data.Models.Restaurant
{ 
	using System.ComponentModel.DataAnnotations;
	public class Brand
	{
        public Brand()
        {
            Restaurants = new HashSet<Restaurant>();
			Menus = new HashSet<Menu>();
		}

        [Key]
		public Guid Id { get; set; }

		[Required]
		public string BrandName { get; set; } = null!;
		[Required]
		public string LogoUrl { get; set; } = null!;
		[Required]
        public string WebsiteUrl { get; set; } = null!;
		[Required]
		public string Description { get; set; } = null!;

		public bool IsActive { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; }

		public virtual ICollection<Menu> Menus { get; set; }
	}
}
