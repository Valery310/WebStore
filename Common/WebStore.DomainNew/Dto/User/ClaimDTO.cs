using System.Collections.Generic;
using System.Security.Claims;
using WebStore.Domain.Dto;

namespace WebStore.Domain.Entities
{
    public abstract class ClaimDto : UserDto
    {
        public IEnumerable<Claim> Claims { get; init; }
    }
}
