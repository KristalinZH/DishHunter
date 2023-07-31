namespace DishHunter.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Restaurant;

    public class SettlementEntityConfiguration : IEntityTypeConfiguration<Settlement>
    {
        public void Configure(EntityTypeBuilder<Settlement> builder)
        {
            builder
                .Property(s => s.IsActive)
                .HasDefaultValue(true);
            builder
                .HasMany(s => s.Restaurants)
                .WithOne(r => r.Settlement)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
