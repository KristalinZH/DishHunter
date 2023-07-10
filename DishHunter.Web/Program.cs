
namespace DishHunter.Web
{
	using Microsoft.EntityFrameworkCore;
	using Data;

	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			//Add database contexts

			var restaurantConnectionString = builder.Configuration.GetConnectionString("RestaurantConnection")
				?? throw new InvalidOperationException("Connection string 'RestaurantConnection' not found.");

			builder.Services.AddDbContext<RestaurantDbContext>(options =>
				options.UseSqlServer(restaurantConnectionString));
			// Add services to the container.
			builder.Services.AddControllersWithViews();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}