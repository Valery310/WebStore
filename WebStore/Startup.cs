using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Middleware;
using WebStore.Services.Interfaces;
using WebStore.Services;
using WebStore.DAL.Context;
using Microsoft.EntityFrameworkCore;
using WebStore.Services.Implementations.Sql;

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
                 
            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            //  services.AddSingleton<IProductData, InMemoryProductData>();
            //  services.AddSingleton<IProductData, SqlProductData>();
            services.AddScoped<IProductData, SqlProductData>();
            // services.AddMvc().AddMvcOptions(mvcOptions => mvcOptions.EnableEndpointRouting = false);
            services.AddDbContext<WebStoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseMiddleware<TestMiddleware>();
            app.UseWelcomePage("/welcome");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // var hello = Configuration["CustomHelloWorld"];
            //  app.Run(async (context) => await context.Response.WriteAsync(hello));
        }
    }
}
