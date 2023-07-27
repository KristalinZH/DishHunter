namespace DishHunter.Web
{
	using DishHunter.Data;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;

	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
				?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));

			builder.Services.AddDefaultIdentity<IdentityUser>(options => 
			{
				options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
				options.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
				options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
				options.Password.RequiredUniqueChars = builder.Configuration.GetValue<int>("Identity:Password:RequiredUniqueChars");
				options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigit"); 
				options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase"); 
				options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric"); 
				options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase"); 
			})
				.AddEntityFrameworkStores<ApplicationDbContext>();

			builder.Services.AddControllersWithViews();

			WebApplication app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			app.Run();
		}
	}
}