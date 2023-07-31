namespace DishHunter.Data.Configurations
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using Models.Restaurant;

	public class MenuItemEntityConfigration : IEntityTypeConfiguration<MenuItem>
	{
		public void Configure(EntityTypeBuilder<MenuItem> builder)
		{
			builder
				.Property(mi => mi.Price)
				.HasPrecision(8, 2);
			builder
				.Property(mi => mi.CreatedOn)
				.HasDefaultValueSql("GETDATE()");
			builder
				.Property(mi => mi.IsActive)
				.HasDefaultValue(true);
			builder
				.HasOne(mi => mi.Menu)
				.WithMany(m => m.MenuItems)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
