using System;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TBS.Models;

namespace WebAPI.Services
{
    public class ItemService
    {
        private readonly IMongoCollection<Item> _Items;

        public static ItemService Instance => m_instance ?? (m_instance = new ItemService());

        private static ItemService m_instance;

        public ItemService()
        {
            var client = new MongoClient(AppConfig.DATABASE_ADRESS);
            var database = client.GetDatabase("TBS");

            _Items = database.GetCollection<Item>("Items");
        }

        public async Task<List<Item>> GetAll()
        {
            return (await _Items.FindAsync(x => true)).ToList();
        }

        public async Task<Item> Get(string id)
        {
            return await _Items.Find(x => x.Id == id).FirstOrDefaultAsync();
        }


        public async Task<Item> Post(Item obj)
        {
            await _Items.InsertOneAsync(obj);
            return obj;
        }

        public async Task<bool> Put(string id, Item item)
        {
            await _Items.ReplaceOneAsync<Item>(x => x.Id == id, item);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            await _Items.DeleteOneAsync(x => x.Id == id);
            return true;
        }
    }
}
