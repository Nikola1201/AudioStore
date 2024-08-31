using AudioStore.DataAccess;
using AudioStore.Models.ViewModels;
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


            // Add HttpContextAccessor used for cookies in ShoppingCartService
            builder.Services.AddHttpContextAccessor();



            // Add services dependency injection
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<CartService>();
            builder.Services.AddScoped<ShoppingCartService>();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
     

            var app = builder.Build();
            app.UseSession();


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
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
