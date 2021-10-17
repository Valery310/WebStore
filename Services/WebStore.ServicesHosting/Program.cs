using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Services.Data;

namespace WebStore.ServicesHosting
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //  await CreateHostBuilder(args).Build().RunAsync();

            var host = CreateHostBuilder(args).Build();

            using (var service_scope = host.Services.CreateScope())
            {
                var initializer = service_scope.ServiceProvider.GetRequiredService<DbInitializer>();
                initializer.Initialize();
            }

            host.Run();

            //var host_builder = CreateHostBuilder(args);
            //var host = host_builder.Build();

            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        var context = services.GetRequiredService<WebStoreContext>();
            //        DbInitializer.Initialize(context);

            //        var roleStore = new RoleStore<IdentityRole>(context);
            //        var roleManager = new RoleManager<IdentityRole>(roleStore,
            //            new IRoleValidator<IdentityRole>[] { },
            //            new UpperInvariantLookupNormalizer(),
            //            new IdentityErrorDescriber(), null);

            //        if (!roleManager.RoleExistsAsync("User").Result)
            //        {
            //            var role = new IdentityRole("User");
            //            var result = roleManager.CreateAsync(role).Result;
            //        }

            //        if (!roleManager.RoleExistsAsync("Administrator").Result)
            //        {
            //            var role = new IdentityRole("Administrator");
            //            var result = roleManager.CreateAsync(role).Result;
            //        }

            //        var userStore = new UserStore<User>(context);
            //        var userManager = new UserManager<User>(userStore,
            //            new OptionsManager<IdentityOptions>(new OptionsFactory<IdentityOptions>(new IConfigureOptions<IdentityOptions>[] { },
            //            new IPostConfigureOptions<IdentityOptions>[] { })),
            //            new PasswordHasher<User>(),
            //            new IUserValidator<User>[] { },
            //            new IPasswordValidator<User>[] { },
            //            new UpperInvariantLookupNormalizer(),
            //            new IdentityErrorDescriber(),
            //            null,
            //            null);

            //        if (userStore.FindByEmailAsync("admin@mail.com", CancellationToken.None).Result == null)
            //        {
            //            var user = new User() { UserName = "Admin", Email = "admin@mail.com" };
            //            var result = userManager.CreateAsync(user, "admin").Result;

            //            if (result == IdentityResult.Success)
            //            {
            //                var roleResult = userManager.AddToRoleAsync(user, "Administrator").Result;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "An error occurred while seeding the database.");
            //    }
            //}

            //await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
