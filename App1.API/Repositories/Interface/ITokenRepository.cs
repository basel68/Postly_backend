using Microsoft.AspNetCore.Identity;

namespace App1.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        public string generateJwtToken(IdentityUser user,List<string> roles);
    }
}
