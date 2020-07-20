using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;

using TBS.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebAPI.Security
{
    public class Authentication
    {
        public static readonly string SecretKey = "WUJy2LVDgIEZD6HWwXqQItVWX38ehUzp";
        public static readonly string Issuer = "Test.com";


        public static async Task<User> GetUser(ClaimsPrincipal claimsPrincipal)
        {
            IEnumerable<Claim> claims = claimsPrincipal.FindAll(x => x != null);

            for (int i = 0; i < claims.Count(); i++)
            {
                Console.WriteLine("type : " + claims.ElementAt(i).Type + " " + claims.ElementAt(i).Value);
            }

            if (claimsPrincipal.HasClaim(c => c.Type == "user.id"))
            {
                Console.WriteLine("NameIdentifier founded !");
                return await Services.UserService.Instance.GetUser(claimsPrincipal.FindFirst(c => c.Type == "user.id").Value);
            }


            Console.WriteLine("NameIdentifier not founded !");
            return null;
        }

        public static string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("user.id", user.Id),
                new Claim("user.username", user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(Issuer,
              Issuer,
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
