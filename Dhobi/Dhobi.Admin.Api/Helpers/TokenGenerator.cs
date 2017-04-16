using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Configuration;
using Dhobi.Core.Manager.DbModels;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Newtonsoft.Json;
using Thinktecture.IdentityModel.Tokens;

namespace Dhobi.Admin.Api.Helpers
{
    public class TokenGenerator
    {
        private const int Validity = 14;
        public string GenerateUserToken(ManagerBasicInformation user)
        {
            try
            {
                var issuer = WebConfigurationManager.AppSettings["issuer"];
                var audience = WebConfigurationManager.AppSettings["aud"];
                var key = WebConfigurationManager.AppSettings["secret"];

                var identity = new ClaimsIdentity("JWT");

                identity.AddClaim(new Claim("userId", user.UserId));
                identity.AddClaim(new Claim("userName", user.UserName));
                identity.AddClaim(new Claim("email", user.Email));
                if (user.Name != null)
                {
                    identity.AddClaim(new Claim("name", user.Name));
                }
                identity.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", user.Roles)));

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
        public ManagerBasicInformation GetUserFromToken(string token)
        {
            try
            {
                var tokenOnly = token.Replace("Bearer", "").Trim();
                var jwtDecoded = JWT.JsonWebToken.Decode(tokenOnly, TextEncodings.Base64Url.Decode(WebConfigurationManager.AppSettings["secret"]));
                var user = JsonConvert.DeserializeObject<ManagerBasicInformation>(jwtDecoded);
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