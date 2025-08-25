using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using PostgressTesting.Models;
using PostgressTesting.Repository.Abstraction;
using PostgressTesting.Repository.Implementation;

namespace PostgressTesting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ----------------------------
            // Configure Services
            // ----------------------------
            builder.Services.AddControllersWithViews();

            // ✅ Configure PostgreSQL DbContext with Retry Policy (EF Core)
            builder.Services.AddDbContext<PostGressSqlContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()
                ));

            // ✅ Register IDbConnection for Dapper
            builder.Services.AddScoped<IDbConnection>(sp =>
                new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ✅ Register Repositories
            builder.Services.AddScoped<IStudent, StudentRepository>();

            // ✅ Fix PostgreSQL timestamp behavior
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var app = builder.Build();

            // ----------------------------
            // Configure Middleware Pipeline
            // ----------------------------
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); // Default HSTS = 30 days
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // ✅ Configure Default Route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Student}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
