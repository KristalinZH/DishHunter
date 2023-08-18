namespace DishHunter.Data.Configurations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using static Common.RolesConstants;

    public class RolesSeedConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder
                .HasData(GetRoles());
        }
        private IdentityRole<Guid>[] GetRoles()
        {
            IdentityRole<Guid> adminRole = new IdentityRole<Guid>()
            {
                Id = Guid.Parse("f39b8190-06f0-46ff-b35f-9e9d6703c5d2"),
                Name = AdminRoleName,
                NormalizedName = AdminRoleName.ToUpper(),
            };
            IdentityRole<Guid> userRole = new IdentityRole<Guid>()
            {
                Id = Guid.Parse("c421e5d4-85c6-4173-a17b-23c735028160"),
                Name = UserRoleName,
                NormalizedName = UserRoleName.ToUpper(),
            };
            return new IdentityRole<Guid>[]
            {
                adminRole,
                userRole
            };
        }
    }
}
