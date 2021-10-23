using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Repository;
using Entities.Models;
using Microsoft.IdentityModel.Tokens;

namespace DataAccess.Abstract
{
    public interface IJwtDal
    {
        Task<JWT> CreateJwtTokenAsync(AppUser user);

        bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken);

        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
