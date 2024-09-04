using SemesterTwo.Services;
using Microsoft.Extensions.DependencyInjection;


namespace SemesterTwo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register custom services
            builder.Services.AddSingleton<BlobService>();
            builder.Services.AddSingleton<TableService>();
            builder.Services.AddSingleton<QueueService>();
            builder.Services.AddSingleton<FileService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
