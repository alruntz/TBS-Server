using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using TBS.Models;
using Newtonsoft.Json;

namespace TBS.APICore
{
    public class APIManager
    {
        public static APIManager Instance => m_instance ?? (m_instance = new APIManager());

        private static APIManager m_instance;
        private string m_token;

        private readonly HttpClient client;

        private const string SERVER_ADRESS = "http://localhost:5000";
        private const string EP_AUTH = "/authentication";
        private const string EP_PLAYERS = "/players";
        private const string EP_USERS = "/users";
        private const string EP_ITEMS = "/items";
        private const string EP_SPELLS = "/spells";
        private const string EP_MAPS = "/maps";
        private const string EP_CHARACTERS = "/characters";
        private const string SCOPE_PUBLIC = "/public";
        private const string SCOPE_PRIVATE = "/private";
        private const string SCOPE_ADMIN = "/admin";

        public APIManager()
        {
            client = CreateClient();
        }

        public void Test()
        {

        }

        public async Task<string> GetToken(string username, string password)
        {
            HttpResponseMessage response = await client.GetAsync(
                String.Format("api" + EP_AUTH + "?username={0}&password={1}", username, password)
            );
            if (response == null) return null;

            string responseText = await response.Content.ReadAsStringAsync();
            if (responseText == null || responseText == string.Empty) return null;

            Dictionary<string, string> conv = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
            if (conv == null || !conv.ContainsKey("token")) return null;

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", conv["token"]);
            m_token = conv["token"];

            return m_token;
        }

        public void SetToken(string token)
        {
            m_token = token;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", m_token);
        }

        public string GetTokenSync(string username, string password)
        {
            HttpResponseMessage response = client.GetAsync(
                String.Format("api" + EP_AUTH + "?username={0}&password={1}", username, password)).Result;
            if (response == null) return null;

            string responseText = response.Content.ReadAsStringAsync().Result;
            if (responseText == null || responseText == string.Empty) return null;

            Dictionary<string, string> conv = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
            if (conv == null || !conv.ContainsKey("token")) return null;

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", conv["token"]);
            m_token = conv["token"];

            return m_token;
        }

        private HttpClient CreateClient()
        {
            HttpClient ret = new HttpClient
            {
                BaseAddress = new Uri(SERVER_ADRESS)
            };

            ret.DefaultRequestHeaders.Accept.Clear();
            ret.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return ret;
        }

        
        #region players

        public async Task<Player> GetPlayer(string id)
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_PUBLIC + EP_PLAYERS + "/" + id);
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = await response.Content.ReadAsStringAsync();
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<Player>(responseText);
        }

        public Player GetPlayerSync(string id)
        {
            HttpResponseMessage response = client.GetAsync("api" + SCOPE_PUBLIC + EP_PLAYERS + "/" + id).Result;
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = response.Content.ReadAsStringAsync().Result;
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<Player>(responseText);
        }

        public async Task<List<Player>> GetPlayers()
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_PUBLIC + EP_PLAYERS);
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = await response.Content.ReadAsStringAsync();
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<List<Player>>(responseText);
        }

        public List<Player> GetPlayersSync()
        {
            HttpResponseMessage response = client.GetAsync("api" + SCOPE_PUBLIC + EP_PLAYERS).Result;
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = response.Content.ReadAsStringAsync().Result;
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<List<Player>>(responseText);
        }

#if USE_ADMIN

        public async Task<bool> UpdatePlayer(string id, Player newPlayer)
        {
            HttpResponseMessage response = await client.PutAsync(
                "api" + SCOPE_ADMIN + EP_PLAYERS + "/" + id,
                new StringContent(JsonConvert.SerializeObject(newPlayer), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public async Task<bool> UpdatePlayerTeam(string id, Team newTeam)
        {
            HttpResponseMessage response = await client.PutAsync(
                "api" + SCOPE_ADMIN + EP_PLAYERS + "/" + id + "/team",
                new StringContent(JsonConvert.SerializeObject(newTeam), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public async Task<bool> UpdatePlayerItems(string id, List<string> itemIds)
        {
            HttpResponseMessage response = await client.PutAsync(
               "api" + SCOPE_ADMIN + EP_PLAYERS + "/" + id + "/items",
               new StringContent(JsonConvert.SerializeObject(itemIds), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

#endif
#if USE_PRIVATE

        public async Task<bool> UpdateMyPlayer(Player player)
        {
            HttpResponseMessage response = await client.PutAsync(
               "api" + SCOPE_PRIVATE + EP_PLAYERS,
               new StringContent(JsonConvert.SerializeObject(player), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public bool UpdateMyPlayerSync(Player player)
        {
            HttpResponseMessage response = client.PutAsync(
               "api" + SCOPE_PRIVATE + EP_PLAYERS,
               new StringContent(JsonConvert.SerializeObject(player), Encoding.UTF8, "application/json")).Result;
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public async Task<bool> UpdateMyPlayerTeam(Team newTeam)
        {
            HttpResponseMessage response = await client.PutAsync(
               "api" + SCOPE_PRIVATE + EP_PLAYERS + "/team",
               new StringContent(JsonConvert.SerializeObject(newTeam), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public bool UpdateMyPlayerTeamSync(Team newTeam)
        {
            HttpResponseMessage response = client.PutAsync(
               "api" + SCOPE_PRIVATE + EP_PLAYERS + "/team",
               new StringContent(JsonConvert.SerializeObject(newTeam), Encoding.UTF8, "application/json")).Result;
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }
#endif
        #endregion


        #region users

        public async Task<User> CreateUser(User user)
        {
            HttpResponseMessage response = await client.PostAsync(
                "api" + SCOPE_PUBLIC + EP_PLAYERS,
                new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = await response.Content.ReadAsStringAsync();
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<User>(responseText);
        }

        public User CreateUserSync(User user)
        {
            HttpResponseMessage response = client.PostAsync(
                "api" + SCOPE_PUBLIC + EP_PLAYERS,
                new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")).Result;
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = response.Content.ReadAsStringAsync().Result;
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<User>(responseText);
        }

#if USE_ADMIN
       
        public async Task<User> GetUser(string id)
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_ADMIN + EP_USERS + "/" + id);
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = await response.Content.ReadAsStringAsync();
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<User>(responseText);
        }
        
        public async Task<List<User>> GetUsers()
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_ADMIN + EP_PLAYERS);
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = await response.Content.ReadAsStringAsync();
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<List<User>>(responseText);
        }

        public async Task<bool> UpdateUser(string id, User user)
        {
            HttpResponseMessage response = await client.PutAsync(
              "api" + SCOPE_ADMIN + EP_USERS + "/" + id,
              new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public async Task<bool> DeleteUser(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync("api" + SCOPE_ADMIN + EP_USERS + "/" + id);
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

#endif
#if USE_PRIVATE

        public async Task<User> GetMyUser()
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_PRIVATE + EP_USERS);
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = await response.Content.ReadAsStringAsync();
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<User>(responseText);
        }

        public User GetMyUserSync()
        {
            HttpResponseMessage response = client.GetAsync("api" + SCOPE_PRIVATE + EP_USERS).Result;
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = response.Content.ReadAsStringAsync().Result;
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<User>(responseText);
        }

        public async Task<bool> UpdateMyUser(User user)
        {
            HttpResponseMessage response = await client.PutAsync(
               "api" + SCOPE_PRIVATE + EP_USERS,
               new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public bool UpdateMyUserSync(User user)
        {
            HttpResponseMessage response = client.PutAsync(
               "api" + SCOPE_PRIVATE + EP_USERS,
               new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")).Result;
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

#endif
        #endregion


        #region items

        public async Task<Item> GetItem(string id)
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_PUBLIC + EP_ITEMS + "/" + id);
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = await response.Content.ReadAsStringAsync();

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<Item>(responseText);
        }

        public Item GetItemSync(string id)
        {
            HttpResponseMessage response = client.GetAsync("api" + SCOPE_PUBLIC + EP_ITEMS + "/" + id).Result;
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = response.Content.ReadAsStringAsync().Result;

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<Item>(responseText);
        }

        public async Task<List<Item>> GetItems()
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_PUBLIC + EP_ITEMS);
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = await response.Content.ReadAsStringAsync();

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<List<Item>>(responseText);
        }

        public List<Item> GetItemsSync()
        {
            HttpResponseMessage response = client.GetAsync("api" + SCOPE_PUBLIC + EP_ITEMS).Result;
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = response.Content.ReadAsStringAsync().Result;

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<List<Item>>(responseText);
        }

#if USE_ADMIN

        public async Task<Item> CreateItem(Item item)
        {
            HttpResponseMessage response = await client.PostAsync(
               "api" + SCOPE_ADMIN + EP_ITEMS,
               new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = await response.Content.ReadAsStringAsync();
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<Item>(responseText);
        }

        public async Task<bool> UpdateItem(string id, Item item)
        {
            HttpResponseMessage response = await client.PutAsync(
             "api" + SCOPE_ADMIN + EP_ITEMS + "/" + id,
             new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public async Task<bool> DeleteItem(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync("api" + SCOPE_ADMIN + EP_ITEMS + "/" + id);
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }
#endif

        #endregion


        #region characters

        public async Task<Character> GetCharacter(string id)
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_PUBLIC + EP_CHARACTERS + "/" + id);

            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;
            string responseText = await response.Content.ReadAsStringAsync();

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<Character>(responseText);
        }

        public Character GetCharacterSync(string id)
        {
            HttpResponseMessage response = client.GetAsync("api" + SCOPE_PUBLIC + EP_CHARACTERS + "/" + id).Result;

            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;
            string responseText = response.Content.ReadAsStringAsync().Result;

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<Character>(responseText);
        }

        public async Task<List<Character>> GetCharacters()
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_PUBLIC + EP_CHARACTERS);

            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;
            string responseText = await response.Content.ReadAsStringAsync();

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<List<Character>>(responseText);
        }

        public List<Character> GetCharactersSync()
        {
            HttpResponseMessage response = client.GetAsync("api" + SCOPE_PUBLIC + EP_CHARACTERS).Result;

            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;
            string responseText = response.Content.ReadAsStringAsync().Result;

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<List<Character>>(responseText);
        }

#if USE_ADMIN
        
        public async Task<Character> CreateCharacter(Character character)
        {
            HttpResponseMessage response = await client.PostAsync(
              "api" + SCOPE_ADMIN + EP_CHARACTERS,
              new StringContent(JsonConvert.SerializeObject(character), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = await response.Content.ReadAsStringAsync();
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<Character>(responseText);
        }

        public async Task<bool> Updatecharacter(string id, Character character)
        {
            HttpResponseMessage response = await client.PutAsync(
             "api" + SCOPE_ADMIN + EP_CHARACTERS + "/" + id,
             new StringContent(JsonConvert.SerializeObject(character), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public async Task<bool> Deletecharacter(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync("api" + SCOPE_ADMIN + EP_CHARACTERS + "/" + id);
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

#endif

        #endregion


        #region spells

        public async Task<Spell> Getspell(string id)
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_PUBLIC + EP_SPELLS + "/" + id);

            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;
            string responseText = await response.Content.ReadAsStringAsync();

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<Spell>(responseText);
        }

        public Spell GetspellSync(string id)
        {
            HttpResponseMessage response = client.GetAsync("api" + SCOPE_PUBLIC + EP_SPELLS + "/" + id).Result;

            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;
            string responseText = response.Content.ReadAsStringAsync().Result;

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<Spell>(responseText);
        }

        public async Task<List<Spell>> GetSpells()
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_PUBLIC + EP_SPELLS);

            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;
            string responseText = await response.Content.ReadAsStringAsync();

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<List<Spell>>(responseText);
        }

        public List<Spell> GetSpellsSync()
        {
            HttpResponseMessage response = client.GetAsync("api" + SCOPE_PUBLIC + EP_SPELLS).Result;

            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;
            string responseText = response.Content.ReadAsStringAsync().Result;

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<List<Spell>>(responseText);
        }

#if USE_ADMIN

        public async Task<Spell> CreateSpell(Spell spell)
        {
            HttpResponseMessage response = await client.PostAsync(
             "api" + SCOPE_ADMIN + EP_SPELLS,
             new StringContent(JsonConvert.SerializeObject(spell), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = await response.Content.ReadAsStringAsync();
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<Spell>(responseText);
        }

        public async Task<bool> UpdateSpell(string id, Spell spell)
        {
            HttpResponseMessage response = await client.PutAsync(
             "api" + SCOPE_ADMIN + EP_SPELLS + "/" + id,
             new StringContent(JsonConvert.SerializeObject(spell), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public async Task<bool> DeleteSpell(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync("api" + SCOPE_ADMIN + EP_SPELLS + "/" + id);
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

#endif

        #endregion


        #region Maps

        public async Task<Map> GetMap(string id)
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_PUBLIC + EP_MAPS + "/" + id);

            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;
            string responseText = await response.Content.ReadAsStringAsync();

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<Map>(responseText);
        }

        public Map GetMapSync(string id)
        {
            HttpResponseMessage response = client.GetAsync("api" + SCOPE_PUBLIC + EP_MAPS + "/" + id).Result;

            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;
            string responseText = response.Content.ReadAsStringAsync().Result;

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<Map>(responseText);
        }

        public async Task<List<Map>> GetMaps()
        {
            HttpResponseMessage response = await client.GetAsync("api" + SCOPE_PUBLIC + EP_MAPS);

            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;
            string responseText = await response.Content.ReadAsStringAsync();

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<List<Map>>(responseText);
        }

        

        public List<Map> GetMapsSync()
        {
            HttpResponseMessage response = client.GetAsync("api" + SCOPE_PUBLIC + EP_MAPS).Result;

            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;
            string responseText = response.Content.ReadAsStringAsync().Result;

            if (responseText == null) return null;
            return JsonConvert.DeserializeObject<List<Map>>(responseText);
        }

#if USE_ADMIN

        public async Task<Map> CreateMap(Map map)
        {
            HttpResponseMessage response = await client.PostAsync(
             "api" + SCOPE_ADMIN + EP_MAPS,
             new StringContent(JsonConvert.SerializeObject(map), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = await response.Content.ReadAsStringAsync();
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<Map>(responseText);
        }

        public Map CreateMapSync(Map map)
        {
            HttpResponseMessage response = client.PostAsync(
             "api" + SCOPE_ADMIN + EP_MAPS,
             new StringContent(JsonConvert.SerializeObject(map), Encoding.UTF8, "application/json")).Result;
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return null;

            string responseText = response.Content.ReadAsStringAsync().Result;
            if (responseText == null) return null;

            return JsonConvert.DeserializeObject<Map>(responseText);
        }

        public async Task<bool> UpdateMap(string id, Map map)
        {
            HttpResponseMessage response = await client.PutAsync(
             "api" + SCOPE_ADMIN + EP_MAPS + "/" + id,
             new StringContent(JsonConvert.SerializeObject(map), Encoding.UTF8, "application/json"));
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public bool UpdateMapSync(string id, Map map)
        {
            HttpResponseMessage response = client.PutAsync(
             "api" + SCOPE_ADMIN + EP_MAPS + "/" + id,
             new StringContent(JsonConvert.SerializeObject(map), Encoding.UTF8, "application/json")).Result;
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public async Task<bool> DeleteMap(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync("api" + SCOPE_ADMIN + EP_MAPS + "/" + id);
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

        public bool DeleteMapSync(string id)
        {
            HttpResponseMessage response = client.DeleteAsync("api" + SCOPE_ADMIN + EP_MAPS + "/" + id).Result;
            if (response == null || response.StatusCode == HttpStatusCode.NotFound) return false;

            return true;
        }

#endif

        #endregion
    }
}
