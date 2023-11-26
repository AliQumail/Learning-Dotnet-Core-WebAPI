using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ITokenRepository
    {
        public string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
