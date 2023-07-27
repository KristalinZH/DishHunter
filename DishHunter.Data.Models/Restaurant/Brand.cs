
namespace DishHunter.Data.Models.Restaurant
{ 
	using System.ComponentModel.DataAnnotations;
	using static Common.EntityValidationConstants.Brand;
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
		[MaxLength(BrandNameMaxLenght)]
		public string BrandName { get; set; } = null!;
		[Required]
		[MaxLength(UrlMaxLenght)]
		public string LogoUrl { get; set; } = null!;
		[Required]
		[MaxLength(UrlMaxLenght)]
		public string WebsiteUrl { get; set; } = null!;
		[Required]
		[MaxLength(DescriptionMaxLenght)]
		public string Description { get; set; } = null!;
		public bool IsActive { get; set; }
        public virtual ICollection<Restaurant> Restaurants { get; set; }
		public virtual ICollection<Menu> Menus { get; set; }
	}
}
