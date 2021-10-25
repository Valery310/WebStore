using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities;
using System;

namespace WebStore.Clients.Services.Users
{
    public static class IdentityExtensions
    {                                      
        public static IServiceCollection AddIdentityWebStoreWebAPIClients(this IServiceCollection services)
        {
            services.AddHttpClient("WebStoreAPI", (s, client) => client.BaseAddress = new Uri(s.GetRequiredService<IConfiguration>()["WebAPI"]))
               .AddTypedClient<IUserStore<User>, UsersClient>()
               .AddTypedClient<IUserRoleStore<User>, UsersClient>()
               .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
               .AddTypedClient<IUserEmailStore<User>, UsersClient>()
               .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
               .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
               .AddTypedClient<IUserClaimStore<User>, UsersClient>()
               .AddTypedClient<IUserLoginStore<User>, UsersClient>()
               .AddTypedClient<IRoleStore<Role>, RolesClient>();

            return services;
        }

        public static IdentityBuilder AddIdentityWebStoreWebAPIClients(this IdentityBuilder builder)
        {
            builder.Services.AddIdentityWebStoreWebAPIClients();

            return builder;
        }
    }
}
