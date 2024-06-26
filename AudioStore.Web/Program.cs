using AudioStore.DataAccess;
using AudioStore.Services;
using AudioStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AudioStore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add ef core context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Add services dependency injection
            builder.Services.AddTransient<ICategoryService, CategoryServices>();
            builder.Services.AddTransient<IManufacturerService, ManufacturerServices>();
            builder.Services.AddTransient<IProductService, ProductServices>();



            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

                context.Items["logger"] = "asd";

                logger.LogInformation("Request received on path {@Path}", context.Request.Path);

                await next.Invoke();

                logger.LogInformation("Request handled on path {@Path}", context.Request.Path);

            });

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
