
namespace DishHunter.Data.Models.RestaurantModels
{
	using System.ComponentModel.DataAnnotations;
	public class Brand
	{
		[Key]
		public Guid Id { get; set; }
		[Required]
		public string BrandName { get; set; } = null!;
		public string? LogoUrl { get; set; }

        public string? Description { get; set; }
    }
}
