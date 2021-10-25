using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Dto.User
{
    public class AddLoginDto
    {
        public Entities.User User { get; set; }
        public UserLoginInfo UserLoginInfo { get; set; }
    }
}
