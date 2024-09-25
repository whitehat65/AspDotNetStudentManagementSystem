using Microsoft.AspNetCore.Identity;

namespace StudentManagementSystem.Repositories
{
    public interface ITokenRepository
    {
        string GenerateJwtToken(IdentityUser identityUser,List<string> roles);
    }
}
