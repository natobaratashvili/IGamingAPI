using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Infrastructure.Security.Jwt
{
    public class Payload
    {
        public Dictionary<string, string> Claims { get; set; }
        public string Jti { get; set; }
        public long ExpirationAt { get; set; }
        public long IssuedAt { get; set; }
        public string Issuer { get; set; }

        
        public string ToJson()
        {
            var builder = new StringBuilder();
            builder.Append('{');


            builder.Append($"\"issuer\":\"{Issuer}\",");
            builder.Append($"\"jti\":\"{Jti}\",");
            builder.Append($"\"iat\":\"{IssuedAt}\",");
            builder.Append($"\"exp\":\"{ExpirationAt}\"");

            if (Claims != null && Claims.Count > 0)
            {
                foreach (var claim in Claims)
                {
                    builder.Append($",\"{claim.Key}\":\"{claim.Value}\"");
                }
            }

            builder.Append('}');

            return builder.ToString();
        }
    }
    }


