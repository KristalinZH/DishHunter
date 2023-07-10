
namespace DishHunter.Data.Models.RestaurantModels
{
	using System.ComponentModel.DataAnnotations;
	public class Restaurant
	{
		[Key]
		public Guid Id { get; set; }
		[Required]
		public string Name { get; set; } = null!;
		[Required]
		public string Address { get; set; } = null!;
		[Required]
		public string PhoneNumber { get; set; } = null!;	
		public string? WebsiteUrl { get; set; }
    }
}
