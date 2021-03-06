using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Clients.Services.Employees;
using WebStore.Clients.Services.Users;
using WebStore.Clients.Services.Orders;
using WebStore.Clients.Services.Products;
using WebStore.Clients.Services.Values;
using WebStore.Interfaces.Api;
using WebStore.Services.Implementations;
using Microsoft.Extensions.Logging;
using WebStore.Services.MiddleWare;
using WebStore.Services.Services.Implementations;
using WebStore.Services.Services;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;
using WebStore.Hubs;

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
            services.AddIdentity<User, Role>().AddIdentityWebStoreWebAPIClients().AddDefaultTokenProviders();

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

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore.GB";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            services.AddScoped<ICartStore, CookieCartStore>();
            services.AddScoped<ICartService, CartService>();


            services.AddHttpClient("WebStoreAPI", client => client.BaseAddress = new Uri(Configuration["WebAPI"]))
               .AddTypedClient<IValuesService, ValuesClient>()
               .AddTypedClient<IEmployeesData, EmployeesClient>()
               .AddTypedClient<IProductData, ProductsClient>()
               .AddTypedClient<IOrdersService, OrdersClient>()
               .SetHandlerLifetime(TimeSpan.FromMinutes(5))// ????????? ??? httpclient ???????? ? ???????? ??? ?? ???????
               .AddPolicyHandler(GetRetryPolicy())// https://habr.com/ru/company/dododev/blog/503376/   ???????? ????????? ???????? ? ?????? ???? ?????? ?? ????????
               .AddPolicyHandler(GetCircuitBreakerPolicy()); // ?????????? ????????????? ??????????? ???????? ? ??????? ?????????????? ???????

            static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int MaxRetryCount = 5, int MaxJitterTime = 1000) 
            {
                var jitter = new Random();
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(5, RetryAttempt => TimeSpan.FromSeconds(Math.Pow(2, RetryAttempt)) + TimeSpan.FromMilliseconds(jitter.Next(0, MaxJitterTime)));
            }

            static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
                HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, TimeSpan.FromSeconds(30));

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp, ILoggerFactory loggerFactory)
        {   
            loggerFactory.AddLog4Net();

            // WebStore.Logger.Log4NetExtensions.AddLog4Net(loggerFactory, "log4net.config");

            //  env.EnvironmentName = "Production";

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithRedirects("~/errorstatus/{0}");
            //app.UseStatusCodePages();

            //app.UseStatusCodePagesWithReExecute("/Home/ErrorStatus", "?statusCode={0}");

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            //app.UseMiddleware<ErrorHandlingMiddleware>();

            // app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            // app.UseWelcomePage("/welcome");
            // app.UseMiddleware<TestMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapFallbackToFile("index.html");
            });

            // var hello = Configuration["CustomHelloWorld"];
            //  app.Run(async (context) => await context.Response.WriteAsync(hello));
        }
    }
}
