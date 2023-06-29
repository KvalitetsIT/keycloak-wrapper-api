using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using KitNugs.Configuration;
using Microsoft.AspNetCore.Authentication;

public class TokenHandler : ITokenHandler
{
    private string _tenantIdAttributeName;
    private ILogger<TokenHandler> _logger;

    public TokenHandler(ILogger<TokenHandler> logger, IServiceConfiguration configuration)
    {
        _logger = logger;
        _tenantIdAttributeName = configuration.GetConfigurationValue(ConfigurationVariables.TenantAttributeName);

    }

    public string ExtractTenantIdFromToken(JwtSecurityToken token)
    {
        try
        {
            return ExtractClaim(token, _tenantIdAttributeName).Value;
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning(_tenantIdAttributeName + " could not be found in jwt token");
            throw new HttpRequestException("Attribute was missing from the requesters jwt-token: " + _tenantIdAttributeName, e, System.Net.HttpStatusCode.Unauthorized);
        }
    }


    private Claim ExtractClaim(JwtSecurityToken token, string claimName)
    {
        var customerIdClaim = token.Claims.Where(x => x.Type.Equals(_tenantIdAttributeName)).First();
        return customerIdClaim;
    }

    public async Task<JwtSecurityToken> getJwt(HttpContext httpContext)
    {
        var accessToken = await httpContext.GetTokenAsync("access_token");
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(accessToken);
        var tokenS = jsonToken as JwtSecurityToken;
        return tokenS;
    }
}