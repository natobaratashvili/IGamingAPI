using IGaming.Core.UsersManagement.Security;
using IGaming.Infrastructure.Security.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace IGaming.Infrastructure.Security.Jwt;

public class JwtProvider  : IJwtProvider
{
    private readonly JwtSettings _jwtSettings;
    public JwtProvider( IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }


    public string GenerateToken(Dictionary<string, string> claims)
    {
        var header = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
        var payload = new Payload
        {
            Jti = Guid.NewGuid().ToString(),
            Claims = claims,
            Issuer = _jwtSettings.Issuer,
            IssuedAt = DateTime.UtcNow.ToUnix(),
            ExpirationAt = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes).ToUnix(),
        }.ToJson();

        var headerEncoded = Base64UrlSafeEncode(Encoding.UTF8.GetBytes(header));
        var payloadEncoded = Base64UrlSafeEncode(Encoding.UTF8.GetBytes(payload));

        var signatureForEncoding = $"{headerEncoded}.{payloadEncoded}";
        var signatureEncoded = Base64UrlSafeEncode(Sign(signatureForEncoding));
        return $"{headerEncoded}.{payloadEncoded}.{signatureEncoded}";
    }

    public bool ValidateToken(string token)
    {
        var tokenParts = token.Split('.');
        if (tokenParts.Length != 3) return false;

        var headerEncoded = tokenParts[0];
        var payloadEncoded = tokenParts[1];
        var providedSignature = tokenParts[2];

        var header = JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlDecode(headerEncoded));
        var payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlDecode(payloadEncoded));

  
        var signatureForEncoding = $"{headerEncoded}.{payloadEncoded}";

        var encodedSignature = Base64UrlSafeEncode(Sign(signatureForEncoding));
      
        if(header["alg"] != "HS256") return false;

        if (encodedSignature != providedSignature) return false;


        var now = DateTime.UtcNow.ToUnix();

        if (payload["exp"] != null && long.Parse( payload["exp"]) < now) return false;
        

        if (payload["iat"] != null && long.Parse(payload["iat"]) > now)   return false;
        
        return true;
    }
    public static Dictionary<string, string> GetClaims(string token )
    {
        var tokenParts = token.Split('.');
        if (tokenParts.Length != 3) return null;
        var payloadEncoded = tokenParts[1];
        var payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlDecode(payloadEncoded));
        return payload;
    }
    private byte[] Sign(string input)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
    }
    private static string Base64UrlDecode(string input)
    {
        input = input.Replace('-', '+').Replace('_', '/');
        switch (input.Length % 4)
        {
            case 0:
                break;
            case 2:
                input += "==";
                break;
            case 3:
                input += "=";
                break;
            default:
                throw new InvalidOperationException("Invalid base64url string");
        }

        var bytes = Convert.FromBase64String(input);
        return Encoding.UTF8.GetString(bytes);
    }
    private string Base64UrlSafeEncode(byte[] input)
    {
        return Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}