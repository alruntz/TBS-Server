using System;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TBS.Models;

namespace WebAPI.Services
{
    public class MapService
    {
        private readonly IMongoCollection<Map> _Maps;

        public MapService()
        {
            var client = new MongoClient(AppConfig.DATABASE_ADRESS);
            var database = client.GetDatabase("TBS");
            _Maps = database.GetCollection<Map>("Maps");
        }

        public async Task<List<Map>> GetAll()
        {
            return (await _Maps.FindAsync(x => true)).ToList();
        }

        public async Task<Map> Get(string id)
        {
            return await _Maps.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Map> Post(Map obj)
        {
            await _Maps.InsertOneAsync(obj);
            return obj;
        }

        public async Task<bool> Put(string id, Map obj)
        {
            await _Maps.ReplaceOneAsync(x => x.Id == id, obj);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            await _Maps.DeleteOneAsync(x => x.Id == id);
            return true;
        }
    }
}
