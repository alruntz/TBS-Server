using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TBS.Models;
using System.Security.Claims;

namespace WebAPI.Services
{
    public class PlayerService
    {
        private readonly IMongoCollection<Player> _players;

        public static PlayerService Instance => m_instance ?? (m_instance = new PlayerService());

        private static PlayerService m_instance;

        public PlayerService()
        {
            var client = new MongoClient(AppConfig.DATABASE_ADRESS);
            var database = client.GetDatabase("TBS");
            _players = database.GetCollection<Player>("Players");
            m_instance = this;
        }

        public async Task<Player> CreatePlayer(User user)
        {
            Player player = new Player
            {
                Name = user.Username,
                UserId = user.Id,
                Team = new Team()
            };

            await _players.InsertOneAsync(player);

            return player;
        }

        public async Task<Player> GetPlayer(string id)
        {
            return await _players.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Player> GetPlayer(ClaimsPrincipal claimsPrincipal)
        {
            User user = await Security.Authentication.GetUser(claimsPrincipal);

            return await _players.Find(x => x.UserId == user.Id).FirstOrDefaultAsync();
        }

        public async Task<List<Player>> GetPlayers()
        {
            return await _players.Find(x => true).ToListAsync();
        }

        public async Task<bool> PutPlayer(string playerId, BsonDocument newPlayer)
        {
            var filter = Builders<Player>.Filter.Eq(x => x.Id, playerId);
            UpdateDefinition<Player> updateDefinition = null;
            BsonDocument model = new Player().ToBsonDocument(); // For get fields availables

            for (int i = 0; i < newPlayer.ElementCount; i++)
            {
                if (updateDefinition == null)
                {
                    if (newPlayer.GetElement(i).Name == "Team")
                        continue;

                    if (model.TryGetElement(newPlayer.GetElement(i).Name, out BsonElement a))
                    {
                        updateDefinition = Builders<Player>.Update.Set(newPlayer.GetElement(i).Name, newPlayer.GetElement(i).Value);
                        continue;
                    }
                }

                if (model.TryGetElement(newPlayer.GetElement(i).Name, out BsonElement b))
                    updateDefinition = updateDefinition.Set(newPlayer.GetElement(i).Name, newPlayer.GetElement(i).Value);
            }

            if (updateDefinition == null)
                return false;


            await _players.UpdateOneAsync(filter, updateDefinition);

            return true;
        }

        public async Task<bool> PutTeam(string playerId, Team team)
        {
            System.Console.WriteLine("Put team");

            // Get if team is valid
            TBSEngine.Controllers.Team teamController = new TBSEngine.Controllers.Team(team);
            if (teamController == null || !teamController.TeamIsValid)
                return false;

            // Get if player exist
            Player player = await _players.Find(x => x.Id == playerId).FirstOrDefaultAsync();
            if (player == null)
                return false;

            if (team.Characters != null)
            {
                // Get if player has the items in team
                for (int i = 0; i < team.Characters.Count; i++)
                {
                    if (team.Characters[i].Items != null)
                    {
                        if (player.Items == null)
                            return false;

                        for (int j = 0; j < team.Characters[i].Items.Count; j++)
                        {
                            if (!player.Items.Contains(team.Characters[i].Items[j]))
                                return false;
                        }
                    }

                    if (team.Characters[i].Spells != null)
                    {
                        for (int j = 0; j < team.Characters[i].Spells.Count; j++)
                        {
                            if (!((await CharacterService.Instance.Get(team.Characters[i].CharacterId)).SpellIds.Contains(team.Characters[i].Spells[j])))
                                return false;
                        }
                    }
                }
            }

            player.Team = team;
            await _players.ReplaceOneAsync(x => x.Id == playerId, player);

            return true;
        }


        public async Task<bool> PutItems(string playerId, List<string> items)
        {
            Player player = await PlayerService.Instance.GetPlayer(playerId);

            if (player == null) { System.Console.WriteLine("PutItemService : player == null");  return false; }

            player.Items = items;
            if ((await _players.ReplaceOneAsync(x => x.Id == playerId, player) == null))
            {
                System.Console.WriteLine("PutItemService : replaceOne failed !");
                return false;
            }

            return true;
        }
    }
}
