using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

public interface ITokenHandler
{
    string ExtractTenantIdFromToken(JwtSecurityToken token);
    Task<JwtSecurityToken> getJwt(HttpContext httpContext);
}