using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping_Store_Bussiness.Services.Implements;
using Shopping_Store_Bussiness.Services.Interfaces;
using Shopping_Store_Data;
using Shopping_Store_Data.Model;
using Shopping_Store_Data.Repositories.Implements;
using Shopping_Store_Data.Repositories.Interfaces;


namespace Shopping_Store
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages().AddSessionStateTempDataProvider();
			builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
			builder.Services.AddScoped<IBrandsRepository, BrandsRepository>();
            builder.Services.AddScoped<IProductsRespository, ProductsRespository>();

			builder.Services.AddScoped<ICategoriesServies, CategoriesService>();
            builder.Services.AddScoped<IBrandsService, BrandsService>();
            builder.Services.AddScoped<IProductsServices, ProductsService>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Th?i gian h?t h?n phiên
                options.Cookie.HttpOnly = true; // Ch? cho phép truy c?p cookie t? máy ch?
                options.Cookie.IsEssential = true; // Cookie c?n thi?t cho ?ng d?ng
            });

			builder.Services.AddIdentity<User,IdentityRole>()
	.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

			builder.Services.Configure<IdentityOptions>(options =>
			{
				// Password settings.
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 4;

				options.User.RequireUniqueEmail = true;
			});
			var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithRedirects("/Errors?statusCode={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
			app.UseAuthentication();
			app.UseRouting();
            app.UseAuthorization();

            app.MapRazorPages();
            var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
            SeedData.SeedingData(context);
			app.Run();
        }
    }
}
