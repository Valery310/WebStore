using System;
using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Dto
{
    public abstract class UserDto
    {
        public WebStore.Domain.Entities.User User { get; init; }
    }

    public class AddLoginDTO : UserDto
    {
        public UserLoginInfo UserLoginInfo { get; init; }
    }

    public class PasswordHashDTO : UserDto
    {
        public string Hash { get; init; }
    }

    public class SetLockoutDTO : UserDto
    {
        public DateTimeOffset? LockoutEnd { get; init; }
    }
}
