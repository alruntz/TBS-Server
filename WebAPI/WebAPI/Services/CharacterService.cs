using System;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TBS.Models;

namespace WebAPI.Services
{
    public class CharacterService
    {
        private readonly IMongoCollection<Character> _Characters;

        public static CharacterService Instance => m_instance ?? (m_instance = new CharacterService());

        private static CharacterService m_instance;

        public CharacterService()
        {
            var client = new MongoClient(AppConfig.DATABASE_ADRESS);
            var database = client.GetDatabase("TBS");

            _Characters = database.GetCollection<Character>("Characters");
            m_instance = this;
        }

        public async Task<List<Character>> GetAll()
        {
            return (await _Characters.FindAsync(x => true)).ToList();
        }

        public async Task<Character> Get(string id)
        {
            return await _Characters.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Character> Post(Character obj)
        {
            await _Characters.InsertOneAsync(obj);
            return obj;
        }

        public async Task<bool> Put(string id, Character obj)
        {
            await _Characters.ReplaceOneAsync(x => x.Id == id, obj);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            await _Characters.DeleteOneAsync(x => x.Id == id);
            return true;
        }
    }
}
