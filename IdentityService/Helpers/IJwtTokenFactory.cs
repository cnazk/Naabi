using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityService.Helpers;

public interface IJwtTokenFactory
{
    JwtSecurityToken GetToken(IEnumerable<Claim> authClaims);
}