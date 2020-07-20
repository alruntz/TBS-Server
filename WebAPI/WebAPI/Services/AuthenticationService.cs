using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TBS.Models;

namespace WebAPI.Services
{
    public class AuthenticationService
    {
        public static AuthenticationService Instance => m_instance ?? (m_instance = new AuthenticationService());

        private static AuthenticationService m_instance;

        public AuthenticationService()
        {
            m_instance = this;
        }

        public async Task<string> GetToken(string username, string password)
        {
            User user = await UserService.Instance.GetUser(username, password);

            if (user == null)
                return username + " " + password + " Not found !";
                //return null;

           //Security.AuthOptions options = Security.Authentication.GetOptions(user);

           return Security.Authentication.GenerateToken(/*options*/ user);
        }
    }
}
