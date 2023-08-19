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
                Id = Guid.Parse("d5b353de-0b76-4168-ba6f-bcfcdb7e3029"),
                UserName = "Administrator",
                FirstName = "Administrator",
                LastName = "Administrator",
                Email = "admin@email.com",
                PhoneNumber = "+359883333444",
                NormalizedEmail= "admin@email.com".ToUpper(),
                NormalizedUserName= "Administrator".ToUpper(),
				SecurityStamp =Guid.NewGuid().ToString()
            };
            var owner1 = new ApplicationUser()
            {
                Id = Guid.Parse("b49d1805-e143-47ed-9b72-7761e20d6c88"),
                UserName = "PeshoTheOwner",
                FirstName = "Pesho",
                LastName = "Peshov",
                Email = "pesho@email.com",
                PhoneNumber = "+359884444333",
                RestaurantOwnerId = Guid.Parse("62152f86-525b-454f-92c8-108cea75c239"),
                NormalizedEmail= "pesho@email.com".ToUpper(),
                NormalizedUserName= "PeshoTheOwner".ToUpper(),
				SecurityStamp = Guid.NewGuid().ToString()
            };
            var owner2 = new ApplicationUser()
            {
                Id = Guid.Parse("9e9d933d-973a-433a-ada2-19e4a7d4a509"),
                UserName = "IvanTheOwner",
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "ivan@email.com",
                PhoneNumber = "+359884444333",
                RestaurantOwnerId = Guid.Parse("62152f86-525b-454f-92c8-108cea75c240"),
                NormalizedEmail= "ivan@email.com".ToUpper(),
				NormalizedUserName= "IvanTheOwner".ToUpper(),
				SecurityStamp = Guid.NewGuid().ToString()
            };
            var regularUser = new ApplicationUser()
            {
                Id = Guid.Parse("8a4d1997-8ace-42ba-aac5-1fe005eabd99"),
                UserName = "MishoTheUser",
                FirstName = "Misho",
                LastName = "Mishov",
                Email = "misho@email.com",
                NormalizedEmail= "misho@email.com".ToUpper(),
                NormalizedUserName= "MishoTheUser".ToUpper(),
                PhoneNumber=null,
				RestaurantOwnerId=null,
				SecurityStamp = Guid.NewGuid().ToString()
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "3333");
            owner1.PasswordHash = hasher.HashPassword(owner1, "2222");
            owner2.PasswordHash = hasher.HashPassword(owner2, "2222");
            regularUser.PasswordHash = hasher.HashPassword(regularUser, "1111");
            List<ApplicationUser> users = new List<ApplicationUser>();
            users.Add(adminUser);
            users.Add(owner1);
            users.Add(owner2);
            users.Add(regularUser);
            return users;
        }
    }
}
