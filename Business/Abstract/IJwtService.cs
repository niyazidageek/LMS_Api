using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.IdentityModel.Tokens;

namespace Business.Abstract
{
    public interface IJwtService
    {
        ClaimsPrincipal GetPrincipalFromToken(string token);

        Task<JWT> CreateJwtTokenAsync(AppUser user);
    }
}
