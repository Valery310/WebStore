using System.Security.Claims;

namespace WebStore.Domain.Dto.User
{
    public class ReplaceClaimsDto
    {
        public Entities.User User { get; set; }
        public Claim Claim { get; set; }
        public Claim NewClaim { get; set; }
    }
}
