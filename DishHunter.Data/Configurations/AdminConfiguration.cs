namespace DishHunter.Data.Configurations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AdminConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            builder
                .HasData(SetAdmin());
        }
        private IEnumerable<IdentityUserRole<Guid>> SetAdmin()
        {
            return new List<IdentityUserRole<Guid>>()
            {
                new IdentityUserRole<Guid>()
                {
                    RoleId = Guid.Parse("f39b8190-06f0-46ff-b35f-9e9d6703c5d2"),
                    UserId = Guid.Parse("d5b353de-0b76-4168-ba6f-bcfcdb7e3029")
                },
				new IdentityUserRole<Guid>()
				{
					RoleId = Guid.Parse("c421e5d4-85c6-4173-a17b-23c735028160"),
					UserId = Guid.Parse("b49d1805-e143-47ed-9b72-7761e20d6c88")
				},
				new IdentityUserRole<Guid>()
				{
					RoleId = Guid.Parse("c421e5d4-85c6-4173-a17b-23c735028160"),
					UserId = Guid.Parse("9e9d933d-973a-433a-ada2-19e4a7d4a509")
				},
				new IdentityUserRole<Guid>()
				{
					RoleId = Guid.Parse("c421e5d4-85c6-4173-a17b-23c735028160"),
					UserId = Guid.Parse("8a4d1997-8ace-42ba-aac5-1fe005eabd99")
				}
			};
        }
    }
}
