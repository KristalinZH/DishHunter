namespace DishHunter.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Restaurant;

    public class MenuEntityConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder
                .Property(m => m.IsActive)
                .HasDefaultValue(true);
            builder
                .HasOne(m => m.Brand)
                .WithMany(b => b.Menus)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasMany(m => m.MenuItems)
                .WithOne(mi => mi.Menu)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
