using System.Collections.Generic;
using System.IO;
using TBS.APICore;

using Newtonsoft.Json;
using System.Threading.Tasks;

namespace TBSEngine
{
    public class Database
    {
        public const int TeamMaxCost = 5000;
        public const int TeamMaxCharacters = 4;

        public static List<Controllers.Spell> Spells => m_spells;
        public static List<Controllers.Character>  Characters => m_characters;
        public static List<Controllers.Player> Players => m_players;
        public static List<Controllers.Item> Items => m_items;

        public static List<TBS.Models.Map> Maps => m_maps;
        public static bool Loaded => m_loaded;

        private static List<Controllers.Spell> m_spells;
        private static List<Controllers.Character> m_characters;
        private static List<Controllers.Player> m_players;
        private static List<Controllers.Item> m_items;
        private static List<TBS.Models.Map> m_maps;

        private static bool m_loaded = false;

        public static async Task<string> Init(string username, string password)
        {
            return await TBS.APICore.APIManager.Instance.GetToken(username, password);
        }

        public static string InitSync(string username, string password)
        {
            return TBS.APICore.APIManager.Instance.GetTokenSync(username, password);
        }

        public static void SetToken()
        {

        }

        public static async Task<bool> LoadAllAsync()
        {
            if (!(await LoadSpells())) return false;
            if (!(await LoadItems())) return false;
            if (!(await LoadCharacters())) return false;
            if (!(await LoadPlayers())) return false;
            if (!(await LoadMaps())) return false;

            m_loaded = true;

            return true;
        }

        public static bool LoadAll()
        {
            if (!LoadSpellsSync()) return false;
            if (!LoadItemsSync()) return false;
            if (!LoadCharactersSync()) return false;
            if (!LoadPlayersSync()) return false;
            if (!LoadMapsSync()) return false;

            m_loaded = true;

            return true;
        }

        public static void LoadAll(
            List<TBS.Models.Spell> spells,
            List<TBS.Models.Character> characters,
            List<TBS.Models.Player> players,
            List<TBS.Models.Item> items)
        {
            //TODO : Maps missing
            m_spells = new List<Controllers.Spell>();
            m_characters = new List<Controllers.Character>();
            m_players = new List<Controllers.Player>();
            m_items = new List<Controllers.Item>();

            for (int i = 0; i < spells.Count; i++) AddSpell(new Controllers.Spell(spells[i]));
            for (int i = 0; i < items.Count; i++) AddItem(new Controllers.Item(items[i]));
            for (int i = 0; i < characters.Count; i++) AddCharacter(new Controllers.Character(characters[i]));
            for (int i = 0; i < players.Count; i++) AddPlayer(new Controllers.Player(players[i]));
        }

        public static void AddSpell(Controllers.Spell spell)
        {
            m_spells.Add(spell);
        }

        public static void AddItem(Controllers.Item item)
        {
            m_items.Add(item);
        }

        public static void AddCharacter(Controllers.Character character)
        {
            m_characters.Add(character);
        }

        public static void AddPlayer(Controllers.Player player)
        {
            m_players.Add(player);
        }

        public static async Task<bool> LoadMaps()
        {
            if ((m_maps = (await APIManager.Instance.GetMaps())) == null)
                return false;

            return true;
        }

        public static bool LoadMapsSync()
        {
            if ((m_maps = (APIManager.Instance.GetMapsSync())) == null)
                return false;

            return true;
        }

        public static async Task<bool> LoadSpells()
        {
            m_spells = new List<Controllers.Spell>();
            List<TBS.Models.Spell> spellModels = null;
            spellModels = await APIManager.Instance.GetSpells();

            if (spellModels == null)
                return false;

            for (int i = 0; i < spellModels.Count; i++)
                m_spells.Add(new Controllers.Spell(spellModels[i]));

            return true;
        }

        public static bool LoadSpellsSync()
        {
            m_spells = new List<Controllers.Spell>();
            List<TBS.Models.Spell> spellModels = null;
            spellModels = APIManager.Instance.GetSpellsSync();

            if (spellModels == null)
                return false;

            for (int i = 0; i < spellModels.Count; i++)
                m_spells.Add(new Controllers.Spell(spellModels[i]));

            return true;
        }

        public static async Task<bool> LoadCharacters()
        {
            m_characters = new List<Controllers.Character>();
            List<TBS.Models.Character> characterModels = null;
            characterModels = await APIManager.Instance.GetCharacters();

            if (characterModels == null)
                return false;
                
            for (int i = 0; i < characterModels.Count; i++)
                m_characters.Add(new Controllers.Character(characterModels[i]));

            return true;
        }

        public static bool LoadCharactersSync()
        {
            m_characters = new List<Controllers.Character>();
            List<TBS.Models.Character> characterModels = null;
            characterModels = APIManager.Instance.GetCharactersSync();

            if (characterModels == null)
                return false;

            for (int i = 0; i < characterModels.Count; i++)
                m_characters.Add(new Controllers.Character(characterModels[i]));

            return true;
        }

        public static async Task<bool> LoadPlayers()
        {
            m_players = new List<Controllers.Player>();
            List<TBS.Models.Player> playerModels = null;
            playerModels = await APIManager.Instance.GetPlayers();

            if (playerModels == null)
                return false;

            for (int i = 0; i < playerModels.Count; i++)
                m_players.Add(new Controllers.Player(playerModels[i]));

            return true;
        }

        public static bool LoadPlayersSync()
        {
            m_players = new List<Controllers.Player>();
            List<TBS.Models.Player> playerModels = null;
            playerModels = APIManager.Instance.GetPlayersSync();

            if (playerModels == null)
                return false;

            for (int i = 0; i < playerModels.Count; i++)
                m_players.Add(new Controllers.Player(playerModels[i]));

            return true;
        }

        public static async Task<bool> LoadItems()
        {
            m_items = new List<Controllers.Item>();
            List<TBS.Models.Item> itemModels = null;
            itemModels = await APIManager.Instance.GetItems();

            if (itemModels == null)
                return false;

            for (int i = 0; i < itemModels.Count; i++)
                m_items.Add(new Controllers.Item(itemModels[i]));

            return true;
        }

        public static bool LoadItemsSync()
        {
            m_items = new List<Controllers.Item>();
            List<TBS.Models.Item> itemModels = null;
            itemModels = APIManager.Instance.GetItemsSync();

            if (itemModels == null)
                return false;

            for (int i = 0; i < itemModels.Count; i++)
                m_items.Add(new Controllers.Item(itemModels[i]));

            return true;
        }
    }
}
