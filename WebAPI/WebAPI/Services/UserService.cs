using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TBS.Models;

namespace WebAPI.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public static UserService Instance => m_instance ?? (m_instance = new UserService());

        private static UserService m_instance;

        public UserService()
        {
            var client = new MongoClient(AppConfig.DATABASE_ADRESS);
            var database = client.GetDatabase("TBS");
            _users = database.GetCollection<User>("Users");
            m_instance = this;
        }

        public async Task<User> GetUser(string id)
        {
            //ProjectionDefinition<User> fieldExcludes = Builders<User>.Projection.Exclude(x => x.Id);
            return await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUser(string username, string password)
        {
            return await _users.Find(x => x.Username == username && x.Password == password).FirstOrDefaultAsync();
        }

        public async Task<User> GetUser(ClaimsPrincipal claimsPrincipal)
        {
            return await Security.Authentication.GetUser(claimsPrincipal);
        }

        public async Task<User> CreateUser(User user)
        {
            if (await (_users.Find(x => x.Username == user.Username).FirstOrDefaultAsync()) != null)
                return null;

            await _users.InsertOneAsync(user);
            User userFounded = await _users.Find(x => x.Id == user.Id).FirstOrDefaultAsync();

            await PlayerService.Instance.CreatePlayer(userFounded);

            return userFounded;
        }

        public async Task<bool> PutUser(BsonDocument newUser, User actualUser, User senderUser)
        {
            if (actualUser == null)
                return false;

            var filter = Builders<User>.Filter.Eq(x => x.Id, actualUser.Id);
            UpdateDefinition<User> updateDefinition = null;
            BsonDocument model = actualUser.ToBsonDocument(); // For get fields availables

            for (int i = 0; i < newUser.ElementCount; i++)
            {
                // Interdit fields
                if (newUser.GetElement(i).Name == "Username"
                    || (newUser.GetElement(i).Name == "Role" && senderUser.Role != (int)UserRole.Administrator))
                    continue;

                if (updateDefinition == null)
                {
                    if (model.TryGetElement(newUser.GetElement(i).Name, out BsonElement a))
                    {
                        updateDefinition = Builders<User>.Update.Set(newUser.GetElement(i).Name, newUser.GetElement(i).Value);
                        continue;
                    }
                }

                if (model.TryGetElement(newUser.GetElement(i).Name, out BsonElement b))
                    updateDefinition = updateDefinition.Set(newUser.GetElement(i).Name, newUser.GetElement(i).Value);
            }

            if (updateDefinition == null)
                return false;
            

            await _users.UpdateOneAsync(filter, updateDefinition);

            return true;
        }
    }
}
