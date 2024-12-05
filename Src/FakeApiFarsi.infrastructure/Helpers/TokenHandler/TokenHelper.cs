using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FakeApiFarsi.infrastructure.Helpers.TokenHandler.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FakeApiFarsi.Infrastructure.Helpers.TokenHandler;

public class TokenHelper
{
    private readonly IConfiguration _config;

    public TokenHelper(IConfiguration config)
    {
        _config = config;
    }

    public TokenModel GenerateToken(UserDataClaim.UserTokenData tokenData)
    {
        if (string.IsNullOrEmpty(tokenData.Username) || string.IsNullOrWhiteSpace(tokenData.Username))
        {
            return new TokenModel();
        }

        var now = DateTime.UtcNow;
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_config["JwtKey"] ?? "");
        var expDate = now.AddHours(6);

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, tokenData.Username),
            new Claim(ClaimTypes.NameIdentifier, tokenData.Id.ToString()),
            new Claim(ClaimTypes.Role, tokenData.RoleId.ToString()),
            new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(new UserDataClaim.UserData()
            {
                Name = tokenData.Name,
                LastName = tokenData.LastName,
                MobileNo = tokenData.MobileNo,
            }))
        };

        var securityKey = new SymmetricSecurityKey(tokenKey);
        var signCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = now,
            Expires = expDate,
            SigningCredentials = signCredential,
            CompressionAlgorithm = CompressionAlgorithms.Deflate,
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        var refreshToken = GenerateRefreshToken();

        return new TokenModel
        {
            Name = tokenData.Username,
            ExpirationDate = expDate,
            Token = tokenHandler.WriteToken(token),
            RefreshToken = refreshToken,
            RefreshTokenExpiration = now.AddDays(15)
        };
    }

    public AccessTokenPayload ValidateAccessToken(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            return new AccessTokenPayload();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_config["JwtKey"] ?? "");

        try
        {
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(accessToken, validationParams, out SecurityToken validatedToken);
            var jwtToken = validatedToken as JwtSecurityToken;

            if (jwtToken == null || jwtToken.Claims == null)
            {
                return new AccessTokenPayload();
            }

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
            var expirationClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "exp");

            if (userIdClaim == null || expirationClaim == null)
            {
                return new AccessTokenPayload();
            }

            var userId = Guid.Parse(userIdClaim.Value);
            var expiration = long.Parse(expirationClaim.Value);

            return new AccessTokenPayload
            {
                UserId = userId,
                Expiration = DateTimeOffset.FromUnixTimeSeconds(expiration).UtcDateTime
            };
        }
        catch (Exception)
        {
            return new AccessTokenPayload();
        }
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}

public class AccessTokenPayload
{
    public Guid UserId { get; set; }
    public DateTime Expiration { get; set; }
}
