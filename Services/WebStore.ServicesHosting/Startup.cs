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
using WebStore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using WebStore.Services.Data;
using System.IO;

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
            var database_type = Configuration["Database"];

            switch (database_type)
            {
                case "SqlServer":
                    services.AddDbContext<WebStoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString(database_type)));
                    break;
                case "Sqlite":
                    services.AddDbContext<WebStoreContext>(options => options.UseSqlite(Configuration.GetConnectionString(database_type), o => o.MigrationsAssembly("WebStore.DAL.Sqlite")));// Использование SQLite
                    break;
                case "InMemory":
                    services.AddDbContext<WebStoreContext>(options => options.UseInMemoryDatabase("WebStore.db"));// Использование SQLite
                    break;
                default:
                    throw new InvalidOperationException($"Тип БД {database_type} не поддерживается");
            }

            services.AddTransient<DbInitializer>();

            // Настройка Identity
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<WebStoreContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;
#endif

                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            //разрешение зависимостей
            services.AddScoped<IEmployeesData, InMemoryEmployeesData>();
            services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<IOrdersService, SqlOrdersService>();


            //настройки корзины
           // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICartService, CookieCartService>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebStore.WebAPI", Version = "v1" });

                const string webstore_api_xml = "WebStore.ServicesHosting.xml";
                const string webstore_domain_xml = "WebStore.Domain.xml";
                const string debug_path = "bin/debug/net5.0";

                //c.IncludeXmlComments("WebStore.WebAPI.xml");
                if (File.Exists(webstore_api_xml))
                    c.IncludeXmlComments(webstore_api_xml);
                else if (File.Exists(Path.Combine(debug_path, webstore_api_xml)))
                    c.IncludeXmlComments(Path.Combine(debug_path, webstore_api_xml));

                if (File.Exists(webstore_domain_xml))
                    c.IncludeXmlComments(webstore_domain_xml);
                else if (File.Exists(Path.Combine(debug_path, webstore_domain_xml)))
                    c.IncludeXmlComments(Path.Combine(debug_path, webstore_domain_xml));
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
