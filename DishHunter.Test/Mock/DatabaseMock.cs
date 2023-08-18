namespace DishHunter.Test.Mock
{
    using Microsoft.EntityFrameworkCore;
    using Data;

    internal class DatabaseMock
    {
        public static ApplicationDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: $"ApplicationDbContext {DateTime.Now.Ticks.ToString()}")
                    .Options;
                return new ApplicationDbContext(dbContextOptions, false);
            }
        }
    }
}
