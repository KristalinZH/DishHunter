namespace DishHunter.Data.Configurations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Account;

    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .HasData(GetUsers());
        }
        private IEnumerable<ApplicationUser> GetUsers()
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var adminUser = new ApplicationUser()
            {
                Id = Guid.Parse("781fe215-36fe-4183-9844-ab5685cc8c24"),
                UserName = "Administrator",
                FirstName = "Administrator",
                LastName = "Administrator",
                Email = "admin@email.com",
                PhoneNumber = "+359883333444",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                NormalizedUserName = "ADMINISTRATOR",
                SecurityStamp=Guid.NewGuid().ToString()
            };
            var owner = new ApplicationUser()
            {
                Id = Guid.Parse("aadb31cc-2d98-4864-84f7-127ea6097123"),
                UserName = "PeshoTheOwner",
                FirstName = "Pesho",
                LastName = "Peshov",
                Email = "pesho@email.com",
                PhoneNumber = "+359884444333",
                NormalizedEmail = "Pesho@EMAIL.COM",
                NormalizedUserName = "PESHOTHEOWNER",
                RestaurantOwnerId = Guid.Parse("10a2102e-f116-4b81-b0a0-32b0d1022cb9"),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var regularUser = new ApplicationUser()
            {
                Id = Guid.Parse("a39c8167-04c4-4235-9fa2-08162c00864d"),
                UserName = "MishoTheUser",
                FirstName = "Misho",
                LastName = "Mishov",
                Email = "misho@email.com",
                NormalizedEmail = "MISHO@EMAIL.COM",
                NormalizedUserName = "MISHOTHEUSER",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "3333");
            owner.PasswordHash = hasher.HashPassword(owner, "2222");
            regularUser.PasswordHash = hasher.HashPassword(regularUser, "1111");
            List<ApplicationUser> users = new List<ApplicationUser>();
            users.Add(adminUser);
            users.Add(owner);
            users.Add(regularUser);
            return users;
        }
    }
}
