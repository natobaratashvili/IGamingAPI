using IGaming.Core.UsersManagement.Security;
using IGaming.Infrastructure.Security.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace IGaming.Infrastructure.Security.Jwt;
/// <summary>
/// Provides functionality for JWT generation and validation.
/// </summary>
public class JwtProvider  : IJwtProvider
{
    private readonly JwtSettings _jwtSettings;
    /// <summary>
    /// Initializes a new instance of the <see cref="JwtProvider"/> class.
    /// </summary>
    /// <param name="jwtOptions">The JWT settings injected via dependency injection.</param>
    public JwtProvider( IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    /// <summary>
    /// Generates a JWT token with the provided claims.
    /// </summary>
    /// <param name="claims">The claims to include in the token.</param>
    /// <returns>The generated JWT token.</returns>
    /// <remarks>
    /// This method generates a JWT token using the provided claims and JWT settings.
    /// It constructs the JWT token by encoding the header and payload, and signing them using HMACSHA256.
    /// </remarks>
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
    /// <summary>
    /// Validates the provided JWT token.
    /// </summary>
    /// <param name="token">The JWT token to validate.</param>
    /// <returns>True if the token is valid; otherwise, false.</returns>
    /// <remarks>
    /// This method validates the provided JWT token by checking its structure, decoding its parts, and verifying the signature.
    /// It also checks the token's expiration and issued timestamps for validity.
    /// </remarks>
    public bool ValidateToken(string token)
    {
        try
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

            if (header["alg"] != "HS256") return false;

            if (encodedSignature != providedSignature) return false;


            var now = DateTime.UtcNow.ToUnix();

            if (payload["exp"] != null && long.Parse(payload["exp"]) < now) return false;


            if (payload["iat"] != null && long.Parse(payload["iat"]) > now) return false;

            return true;
        } catch { return false; }
    }
    /// <summary>
    /// Extracts the claims from the provided JWT token.
    /// </summary>
    /// <param name="token">The JWT token from which to extract claims.</param>
    /// <returns>The claims extracted from the token.</returns>
    /// <remarks>
    /// This method parses the provided JWT token and extracts the claims embedded in its payload.
    /// </remarks>
    public static Dictionary<string, string>? GetClaims(string token )
    {
        var tokenParts = token.Split('.');
        if (tokenParts.Length != 3) return null;
        var payloadEncoded = tokenParts[1];
        var payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlDecode(payloadEncoded));
        return payload;
    }
    /// <summary>
    /// Computes the HMACSHA256 signature for the provided input string.
    /// </summary>
    /// <param name="input">The input string to sign.</param>
    /// <returns>The computed HMACSHA256 signature.</returns>
    /// <remarks>
    /// This method computes the HMACSHA256 signature for the provided input string using the secret key specified in the JWT settings.
    /// It utilizes the HMACSHA256 algorithm to create a cryptographic hash-based message authentication code (HMAC) for data integrity and authenticity.
    /// </remarks>
    private byte[] Sign(string input)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
    }
    /// <summary>
    /// Decodes a base64url-encoded string to its original form.
    /// </summary>
    /// <param name="input">The base64url-encoded string to decode.</param>
    /// <returns>The decoded original string.</returns>
    /// <remarks>
    /// This method converts a base64url-encoded string to its original form.
    /// It replaces the characters used for URL-safe encoding (- and _) with their base64 counterparts before decoding.
    /// </remarks>
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
    /// <summary>
    /// Encodes a byte array into a base64url-encoded string.
    /// </summary>
    /// <param name="input">The byte array to encode.</param>
    /// <returns>The base64url-encoded string.</returns>
    /// <remarks>
    /// This method converts a byte array into a base64url-encoded string.
    /// It removes padding characters (=) and replaces the URL-unsafe characters (+ and /) with their safe counterparts.
    /// </remarks>
    private string Base64UrlSafeEncode(byte[] input)
    {
        return Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}