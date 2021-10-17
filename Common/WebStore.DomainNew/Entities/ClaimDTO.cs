using System.Collections.Generic;
using System.Security.Claims;
using WebStore.Domain.Dto;

namespace WebStore.Domain.Entities
{
    public abstract class ClaimDTO : UserDto
    {
        public IEnumerable<Claim> Claims { get; init; }
    }

    public class AddClaimDTO : ClaimDTO { }

    public class RemoveClaimDTO : ClaimDTO { }

    public class ReplaceClaimDTO : UserDto
    {
        public Claim Claim { get; init; }

        public Claim NewClaim { get; init; }
    }
}
