using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using FeedThem.Models;
using FeedThemServer.Repositories;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using FeedThem.Authentication;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FeedThem.Controllers
{
    /// <summary>
    /// Authorization controller. Users retrieve token via it
    /// </summary>
    public class AuthorizationController : Controller
    {
        private UserRepository repository = new UserRepository();
        public AuthorizationController()
        {
        }

        [HttpPost]
        public async Task Token()
        {
            var username = Request.Form["user"];
            var password = Request.Form["password"];

            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password");
                return;
            }
            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token= encodedJwt,
                username= identity.Name
            };
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response,
                                                                  new JsonSerializerSettings { Formatting = Formatting.Indented })
                                     );
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var encryptedTuple = this.GetHashedCredentials(username, password);
            User user = repository.GetUserByHashedCredentials(encryptedTuple.hashedCredentials, encryptedTuple.salt);
            if (user != null)
            {
                var claims = new List<Claim> {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                };
                return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            }
            return null;
        }

        private (string hashedCredentials, byte[] salt) GetHashedCredentials(string username, string password)
        {
            byte[] salt = new byte[128 / 8];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashedCredentials= Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return (hashedCredentials, salt);

        }
    }
}
