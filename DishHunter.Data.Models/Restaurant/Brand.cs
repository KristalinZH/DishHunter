
namespace DishHunter.Data.Models.Restaurant
{
    using DishHunter.Data.Models.Account;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
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
		[ForeignKey(nameof(RestaurantOwner))]
		public Guid RestaurantOwnerId { get; set; }
		public virtual RestaurantOwner RestaurantOwner { get; set; } = null!;
		public virtual ICollection<Restaurant> Restaurants { get; set; }
		public virtual ICollection<Menu> Menus { get; set; }
	}
}
