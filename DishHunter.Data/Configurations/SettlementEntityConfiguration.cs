namespace DishHunter.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Restaurant;
    using Newtonsoft.Json;
    using System.Reflection;

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
            builder
                .HasData(ExtractSettlements());
        }
        private Settlement[] ExtractSettlements()
        {
            string pathToTheResourceFile = Path.GetFullPath(@$"{Directory.GetCurrentDirectory()}\..\DishHunter.Data\SeedingResources\Settlements.json");
            string json = File.ReadAllText(pathToTheResourceFile);
            Settlement[] settlements = JsonConvert.DeserializeObject<Settlement[]>(json)!;
            for (int i = 0; i < settlements.Length; i++)
                settlements[i].Id = i + 1;
            return settlements;
        }
    }
}
