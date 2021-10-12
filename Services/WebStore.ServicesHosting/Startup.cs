using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;
using WebStore.Services.Implementations.Sql;
using WebStore.Services.Implementations;

namespace WebStore.ServicesHosting
{
    public record Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           // services.AddControllersWithViews();

            services.AddControllers();
            //var database_type = Configuration["Database"];

            //switch (database_type)
            //{
            //    case "SqlServer":
            //        services.AddDbContext<WebStoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString(database_type)));
            //        break;
            //    case "Sqlite":
            //        services.AddDbContext<WebStoreContext>(options => options.UseSqlite(Configuration.GetConnectionString(database_type), o => o.MigrationsAssembly("WebStore.DAL.Sqlite")));// Использование SQLite
            //        break;
            //    case "InMemory":
            //        services.AddDbContext<WebStoreContext>(options => options.UseInMemoryDatabase("WebStore.db"));// Использование SQLite
            //        break;
            //    default:
            //        throw new InvalidOperationException($"Тип БД {database_type} не поддерживается");
            //}

            services.AddDbContext<WebStoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //разрешение зависимостей
            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
        //    services.AddScoped<IProductData, SqlProductData>();
        //    services.AddScoped<IOrdersService, SqlOrdersService>();

            //настройки корзины
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      //      services.AddScoped<ICartService, CookieCartService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebStore.ServicesHosting", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebStore.ServicesHosting v1"));
            }
          //  app.UseMvc();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
