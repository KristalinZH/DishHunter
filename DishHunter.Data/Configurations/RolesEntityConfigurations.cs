namespace DishHunter.Data.Configurations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using static Common.RolesConstants;

    public class RolesEntityConfigurations : IEntityTypeConfiguration<IdentityRole<Guid>>
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
                Id = Guid.NewGuid(),
                Name = AdminRoleName,
                NormalizedName = AdminRoleName.ToUpper(),
            };
            IdentityRole<Guid> userRole = new IdentityRole<Guid>()
            {
                Id = Guid.NewGuid(),
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
