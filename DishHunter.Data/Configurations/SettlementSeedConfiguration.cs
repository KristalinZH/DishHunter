namespace DishHunter.Data.Configurations
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Restaurant;
    using Newtonsoft.Json;

    internal class SettlementSeedConfiguration : IEntityTypeConfiguration<Settlement>
    {
        public void Configure(EntityTypeBuilder<Settlement> builder)
        {
            builder
                .HasData(ExtractSettlements());
            Settlement settlement = new Settlement()
            {
                Id = 5248,
                Region = "mANGALOVO",
                SettlementName = "Asparuhovo",
                IsActive = true
            };
            builder.HasData(settlement);
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
