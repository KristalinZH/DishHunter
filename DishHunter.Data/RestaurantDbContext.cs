namespace DishHunter.Data
{
	using Microsoft.EntityFrameworkCore;
	public class RestaurantDbContext : DbContext
	{
		public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
			: base(options) { }

	}
}
