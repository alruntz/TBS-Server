using System;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq;
using Newtonsoft.Json.Linq;
using TBS.Models;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class SpellService
    {
        private readonly IMongoCollection<Spell> _Spells;

        public static SpellService Instance => m_instance ?? (m_instance = new SpellService());

        private static SpellService m_instance;

        public SpellService()
        {
            var client = new MongoClient(AppConfig.DATABASE_ADRESS);
            var database = client.GetDatabase("TBS");

            _Spells = database.GetCollection<Spell>("Spells");
        }

        public async Task<List<Spell>> GetAll()
        {
            return (await _Spells.FindAsync(x => true)).ToList();
        }

        public async Task<Spell> Get(string id)
        {
            return await _Spells.Find(x => x.Id == id).FirstOrDefaultAsync();
        }


        public async Task<Spell> Post(Spell obj)
        {
            await _Spells.InsertOneAsync(obj);
            return obj;
        }

        public async Task<bool> Put(string id, Spell obj)
        {
            await _Spells.ReplaceOneAsync(x => x.Id == id, obj);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            await _Spells.DeleteOneAsync(x => x.Id == id);
            return true;
        }
    }
}
