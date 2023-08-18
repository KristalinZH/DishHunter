namespace DishHunter.Test.Services
{
    using BaseTest;
    using DishHunter.Services.Data.Models.RestaurantOwner;
    using Microsoft.EntityFrameworkCore;

    internal class RestaurantOwnerService:BaseTestClass
    {
        [Test]
        public async Task CreateOwnerByUserResult()
        {
            string userId = "d740d68c-ba8e-448c-b2e4-f0db5b60c889";
            RestaurantOwnerPostTransferModel tm = new RestaurantOwnerPostTransferModel()
            {
                PhoneNumber="0999"
            };
            await ownerService.CreateOwnerByUserId(userId, tm);
            var owners = await dbContext.RestaurantOwners.ToArrayAsync();
            var result = owners.Count();
            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        public async Task GetOwnerIdByUserTrueResult()
        {
            string userId = "33f33961-b87f-4398-ba3a-e69561a9d346";
            var result = await ownerService.GetOwnerIdByUserId(userId);
            Assert.That(result, Is.Not.EqualTo(null));
        }
        [Test]
        public async Task GetOwnerIdByUserFalseResult()
        {
            string userId = "d740d68c-ba8e-448c-b2e4-f0db5b60c889";
            var result = await ownerService.GetOwnerIdByUserId(userId);
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public async Task OwnerExistsByPhoneNumberTrueResult()
        {
            string phone = "0996";
            bool result = await ownerService.OwnerExistsByPhoneNumberAsync(phone);
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task OwnerExistsByPhoneNumberFalseResult()
        {
            string phone = "0999"; 
            var result = await ownerService.OwnerExistsByPhoneNumberAsync(phone);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task OwnerExistsByUserIdTrueResult()
        {
            string userId = "33f33961-b87f-4398-ba3a-e69561a9d346";
            bool result = await ownerService.OwnerExistsByUserIdAsync(userId);
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task OwnerExistsByUserIdFalseResult()
        {
            string userId = "d740d68c-ba8e-448c-b2e4-f0db5b60c889";
            var result = await ownerService.OwnerExistsByUserIdAsync(userId);
            Assert.That(result, Is.EqualTo(false));
        }
    }
}
