using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    namespace WebStore.Interfaces.Services.Interfaces
    {
        public interface IRolesClient : IRoleStore<Role> { }
    }
}
