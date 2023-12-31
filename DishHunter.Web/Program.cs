namespace DishHunter.Web
{
	using Microsoft.EntityFrameworkCore;
	using Data;
	using Data.Models.Account;
    using Services.Data.Interfaces;
	using Infrastructrure.Extensions;
	using Infrastructrure.ModelBinders;
    using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;

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

			builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
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
				.AddRoles<IdentityRole<Guid>>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			builder.Services.AddControllersWithViews();
			builder.Services.AddHttpClient();
			builder.Services.AddAplicationServices(typeof(IBrandService));
			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});
			builder.Services.Configure<IISServerOptions>(opt =>
			{
				opt.AllowSynchronousIO = true; 
			});
			builder.Services
				.AddControllersWithViews()
				.AddMvcOptions(opt =>
				{
					opt.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
				});
			WebApplication app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error/500");
				app.UseStatusCodePagesWithRedirects("Home/Error?statuscode={0}");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

            app.UseEndpoints(config =>
            {
                config.MapControllerRoute(
                    name: "areas",
                    pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}");

				config.MapControllerRoute(
					name: "default",
					pattern: "/{controller=Home}/{action=Index}/{id?}");

				config.MapDefaultControllerRoute();

                app.MapRazorPages();
            });

			app.Run();
		}
	}
}