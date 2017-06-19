using System;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Web.Configuration;
using Dhobi.Core.Manager.DbModels;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Newtonsoft.Json;
using Thinktecture.IdentityModel.Tokens;
using Dhobi.Core.UserModel.DbModels;
using Dhobi.Core.Dobi.DbModels;

namespace Dhobi.Api.Helpers
{
    public class TokenGenerator
    {
        private const int Validity = 14;
        public string GenerateUserToken(User user)
        {
            try
            {
                var issuer = WebConfigurationManager.AppSettings["issuer"];
                var audience = WebConfigurationManager.AppSettings["aud"];
                var key = WebConfigurationManager.AppSettings["secret"];

                var identity = new ClaimsIdentity("JWT");

                identity.AddClaim(new Claim("userId", user.UserId));
                identity.AddClaim(new Claim("name", user.Name));
                identity.AddClaim(new Claim("phoneNumber", user.PhoneNumber));
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));

                var now = DateTime.UtcNow;
                var expires = now.AddDays(Validity);
                var symmetricKeyAsBase64 = key;

                var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

                var signingKey = new HmacSigningCredentials(keyByteArray);
                var token = new JwtSecurityToken(issuer, audience, identity.Claims, now, expires, signingKey);

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.WriteToken(token);

                return jwtToken;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public string GenerateDobiToken(DobiBasicInformation dobi)
        {
            try
            {
                var issuer = WebConfigurationManager.AppSettings["issuer"];
                var audience = WebConfigurationManager.AppSettings["aud"];
                var key = WebConfigurationManager.AppSettings["secret"];

                var identity = new ClaimsIdentity("JWT");

                identity.AddClaim(new Claim("dobiId", dobi.DobiId));
                identity.AddClaim(new Claim("name", dobi.Name));
                identity.AddClaim(new Claim("phone", dobi.Phone));
                identity.AddClaim(new Claim("photo", dobi.Photo ?? ""));
                identity.AddClaim(new Claim(ClaimTypes.Role, "dobi"));

                var now = DateTime.UtcNow;
                var expires = now.AddDays(Validity);
                var symmetricKeyAsBase64 = key;

                var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

                var signingKey = new HmacSigningCredentials(keyByteArray);
                var token = new JwtSecurityToken(issuer, audience, identity.Claims, now, expires, signingKey);

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.WriteToken(token);

                return jwtToken;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public User GetUserFromToken(string token)
        {
            try
            {
                var tokenOnly = token.Replace("Bearer", "").Trim();
                var jwtDecoded = JWT.JsonWebToken.Decode(tokenOnly, TextEncodings.Base64Url.Decode(WebConfigurationManager.AppSettings["secret"]));
                var user = JsonConvert.DeserializeObject<User>(jwtDecoded);
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DobiBasicInformation GetDobiFromToken(string token)
        {
            try
            {
                var tokenOnly = token.Replace("Bearer", "").Trim();
                var jwtDecoded = JWT.JsonWebToken.Decode(tokenOnly, TextEncodings.Base64Url.Decode(WebConfigurationManager.AppSettings["secret"]));
                var user = JsonConvert.DeserializeObject<DobiBasicInformation>(jwtDecoded);
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long GetTokenValidity()
        {
            try
            {
                var date = DateTime.Now.AddDays(Validity);
                var time = date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                return (long)time;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}