using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DataAccess.Concrete
{
    public class EFJwtDal : IJwtDal
    {
        private readonly JWTConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly UserManager<AppUser> _userManager;


        public EFJwtDal(IOptions<JWTConfig> jwtConfig, TokenValidationParameters tokenValidationParameters,
            UserManager<AppUser> userManager)
        {
            _jwtConfig = jwtConfig.Value;
            _tokenValidationParameters = tokenValidationParameters;
            _userManager = userManager;
        }

        public async Task<JWT> CreateJwtTokenAsync(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var expiryDate = DateTime.UtcNow.AddMinutes(_jwtConfig.DurationInMinutes);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: expiryDate,
                signingCredentials: signingCredentials);


            return new JWT
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExpiryDate = expiryDate
            };
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }

            catch
            {
                return null;
            }
        }

        public bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
