using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.DAL.Context;
using Microsoft.EntityFrameworkCore;
using WebStore.Services.Implementations.Sql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using WebStore.Services.Implementations;
using System;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Api;
using WebStore.Clients.Services.Values;
using WebStore.Clients.Services.Employees;
using WebStore.Clients.Services.Products;
using WebStore.Clients.Services.Orders;

namespace WebStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            // ��������� ���������� �������
            // services.AddTransient<IValuesService, ValuesClient>();
            services.AddHttpClient("WebStoreAPI", client => client.BaseAddress = new Uri(Configuration["WebAPI"]))
                .AddTypedClient<IValuesService, ValuesClient>()
                .AddTypedClient<IEmployeesData, EmployeesClient>()
                .AddTypedClient<IProductData, ProductsClient>()
                .AddTypedClient<IOrdersService, OrdersClient>();

            //  services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
         //   services.AddSingleton<IEmployeesData, EmployeesClient>();
        //    services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<IOrdersService, SqlOrdersService>();



          


            var database_type = Configuration["Database"];

            switch (database_type)
            {
                case "SqlServer":
                    services.AddDbContext<WebStoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString(database_type)));
                    break;
                case "Sqlite":
                    services.AddDbContext<WebStoreContext>(options => options.UseSqlite(Configuration.GetConnectionString(database_type), o => o.MigrationsAssembly("WebStore.DAL.Sqlite")));// ������������� SQLite
                    break;
                case "InMemory":
                    services.AddDbContext<WebStoreContext>(options => options.UseInMemoryDatabase("WebStore.db"));// ������������� SQLite
                    break;
                default:
                    throw new InvalidOperationException($"��� �� {database_type} �� ��������������");
            }


            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<WebStoreContext>().AddDefaultTokenProviders();

            //services.AddIdentityCore<User>().AddEntityFrameworkStores<WebStoreContext>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options => 
            {
                //password
                options.Password.RequiredLength = 6;
                //lockout
                options.Lockout.DefaultLockoutTimeSpan = System.TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;
                //user
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options => 
            {
                //Cookie
                options.Cookie.HttpOnly = true;
              // options.Cookie.Expiration = System.TimeSpan.FromDays(150);
                options.ExpireTimeSpan = System.TimeSpan.FromDays(150);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICartService, CookieCartService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
           
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWelcomePage("/welcome");
            app.UseAuthentication();

         //   app.UseMiddleware<TestMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // var hello = Configuration["CustomHelloWorld"];
            //  app.Run(async (context) => await context.Response.WriteAsync(hello));
        }
    }
}
