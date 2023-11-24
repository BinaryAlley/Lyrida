#region ========================================================================= USING =====================================================================================
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Infrastructure.Core.Services.Time;
using Lyrida.Infrastructure.Common.Configuration;
#endregion

namespace Lyrida.Infrastructure.Core.Authentication;

/// <summary>
/// Generates JWT tokens
/// </summary>
/// <remarks>
/// Creation Date: 07th of July, 2023
/// </remarks>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IAppConfig appConfig;
    private readonly ICryptography cryptographyService;
    private readonly IDateTimeProvider dateTimeProviderService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="dateTimeProviderService">Injected service for time related functionality.</param>
    public JwtTokenGenerator(IDateTimeProvider dateTimeProviderService, IAppConfig appConfig, ICryptography cryptographyService)
    {
        this.appConfig = appConfig;
        this.cryptographyService = cryptographyService;
        this.dateTimeProviderService = dateTimeProviderService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Generates a new JWT token.
    /// </summary>
    /// <param name="id">The id of the user for which to generate the token.</param>
    /// <param name="username">The username of the user for which to generate the token.</param>
    /// <returns>The generated JWT token.</returns>
    public string GenerateToken(string id, string username)
    {
        // use a symmetric key approach
        var securityKey = cryptographyService.Decrypt(appConfig.JwtSettings!.SecretKey!);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, id),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(JwtRegisteredClaimNames.Jti, id.ToString())
        };
        var securityToken = new JwtSecurityToken(
            issuer: appConfig.JwtSettings.Issuer, 
            audience: appConfig.JwtSettings.Audience, 
            expires: dateTimeProviderService.UtcNow.AddMinutes(appConfig.JwtSettings.ExpiryMinutes),  
            claims: claims, 
            signingCredentials: signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
    #endregion
}