namespace DishHunter.Data
{
	using System.Reflection;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Models.Account;
	using Models.Restaurant;
    using DishHunter.Data.Configurations;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
	{
		private bool _seedDb;
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, bool seedDb = true)
			: base(options)
		{
			_seedDb = seedDb;
		}
		public DbSet<RestaurantOwner> RestaurantOwners { get; set; } = null!;
		public DbSet<Brand> Brands { get; set; } = null!;
		public DbSet<Restaurant> Restaurants { get; set; } = null!;
		public DbSet<Settlement> Settlements { get; set; } = null!;
		public DbSet<Category> Categories { get; set; } = null!;
		public DbSet<Menu> Menus { get; set; } = null!;
		public DbSet<MenuItem> MenuItems { get; set; } = null!;
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new BrandEntityConfiguration());
			builder.ApplyConfiguration(new CategoryEntityConfiguration());
			builder.ApplyConfiguration(new MenuEntityConfiguration());
			builder.ApplyConfiguration(new MenuItemEntityConfigration());
			builder.ApplyConfiguration(new RestaurantEntityConfiguration());
			builder.ApplyConfiguration(new RestaurantOwnerEntityConfiguration());
			builder.ApplyConfiguration(new SettlementEntityConfiguration());
			if (_seedDb)
			{
				builder.ApplyConfiguration(new CategorySeedConfiguration());
				builder.ApplyConfiguration(new SettlementSeedConfiguration());
				builder.ApplyConfiguration(new RolesSeedConfiguration());
				builder.ApplyConfiguration(new UserConfiguration());
                builder.ApplyConfiguration(new RestaurantOwnerSeedConfiguration());
                builder.ApplyConfiguration(new AdminConfiguration());
				builder.ApplyConfiguration(new BrandSeedConfiguration());
                builder.ApplyConfiguration(new RestaurantSeedConfuration());
                builder.ApplyConfiguration(new MenuSeedConfiguration());
                builder.ApplyConfiguration(new MenuItemSeedConfiguration());
            }
			base.OnModelCreating(builder);
		}

	}
}
