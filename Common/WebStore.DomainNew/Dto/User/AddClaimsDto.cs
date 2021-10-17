using System.Collections.Generic;
using System.Security.Claims;

namespace WebStore.Domain.Dto.User
{
    public class AddClaimsDto
    {
        public Entities.User User { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}
