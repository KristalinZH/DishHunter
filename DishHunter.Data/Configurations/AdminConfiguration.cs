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
        private IdentityUserRole<Guid> SetAdmin()
        {
            return new IdentityUserRole<Guid>()
            {
                RoleId = Guid.Parse("f39b8190-06f0-46ff-b35f-9e9d6703c5d2"),
                UserId = Guid.Parse("781fe215-36fe-4183-9844-ab5685cc8c24")
            };
        }
    }
}
