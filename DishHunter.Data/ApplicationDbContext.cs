namespace DishHunter.Data
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Models.Account;
	using Models.Restaurant;
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) { }
		public DbSet<RestaurantOwner> RestaurantOwners { get; set; } = null!;
		public DbSet<Brand> Brands { get; set; } = null!;
		public DbSet<Restaurant> Restaurants { get; set; } = null!;
		public DbSet<Settlement> Settlements { get; set; } = null!;
		public DbSet<Category> Categories { get; set; } = null!;
		public DbSet<Menu> Menus { get; set; } = null!;
		public DbSet<MenuItem> MenuItems { get; set; } = null!;
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}

	}
}
