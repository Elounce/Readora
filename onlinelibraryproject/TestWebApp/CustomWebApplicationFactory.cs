using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using webapi.Model;

namespace TestWebApp
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Удаляем текущий контекст БД
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MkarpovCourseworkLibraryContext>));
                if (descriptor != null) services.Remove(descriptor);

                // Добавляем новый контекст с InMemory БД
                services.AddDbContext<MkarpovCourseworkLibraryContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Инициализация тестовых данных (если нужно)
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<MkarpovCourseworkLibraryContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
