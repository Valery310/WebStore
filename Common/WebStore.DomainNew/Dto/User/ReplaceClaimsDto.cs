using System.Security.Claims;

namespace WebStore.Domain.Dto.User
{
    public class ReplaceClaimDto : UserDto
    {
        public Claim Claim { get; init; }

        public Claim NewClaim { get; init; }
    }
}
