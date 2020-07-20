using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TBSEngine.Controllers
{
    public class Player
    {
        public string Name => m_playerModel.Name;
        public Controllers.Team Team => m_team;

        public TBS.Models.Player PlayerModel => m_playerModel;
        public List<Controllers.Item> Items => m_items;


        //private readonly List<Team> m_characterControllers;
        private readonly TBS.Models.Player m_playerModel;
        private readonly Controllers.Team m_team;
        private readonly List<Controllers.Item> m_items;

        public Player(TBS.Models.Player playerModel)
        {
            m_playerModel = playerModel;
            m_team = new Controllers.Team(playerModel.Team);
            m_items = new List<Controllers.Item>();

            for (int i = 0; i < playerModel.Items.Count; i++)
            {
                m_items.Add(Database.Items.Find(x => x.Id == playerModel.Items[i]));
            }
        }
    }

    public class Team
    {
        public List<Controllers.CharacterTeam> Characters => m_characters;
        public TBS.Models.Team TeamModel => m_teamModel;
        public int Cost => GetTotalCost();
        public bool TeamIsValid => Cost <= Database.TeamMaxCost && m_characters.Count <= Database.TeamMaxCharacters;

        private readonly TBS.Models.Team m_teamModel;
        private readonly List<Controllers.CharacterTeam> m_characters;

        public Team(TBS.Models.Team team)
        {
            m_teamModel = team;
            m_characters = new List<CharacterTeam>();

            if (team.Characters != null)
            {
                for (int i = 0; i < team.Characters.Count; i++)
                {
                    Characters.Add(new CharacterTeam(team.Characters[i]));
                }
            }
        }

        public bool AddCharacter(Controllers.Character character)
        {
            if (Characters.Count < 4)
            {
                if (m_teamModel.Characters == null)
                    m_teamModel.Characters = new List<TBS.Models.CharacterTeam>();
                TBS.Models.CharacterTeam characterTeamModel = new TBS.Models.CharacterTeam() { CharacterId = character.Id };
                m_teamModel.Characters.Add(characterTeamModel);
                m_characters.Add(new Controllers.CharacterTeam(characterTeamModel));
                return true;
            }

            return false;
        }

        public void RemoveCharacter(Controllers.CharacterTeam characterTeam)
        {
            m_characters.Remove(characterTeam);
            TBS.Models.CharacterTeam characterTeamModel = m_teamModel.Characters.Find(x => x.CharacterId == characterTeam.CharacterController.Id);
            m_teamModel.Characters.Remove(characterTeamModel);
        }

        private int GetTotalCost()
        {
            int ret = 0;

            if (m_characters != null)
            {
                for (int i = 0; i < m_characters.Count; i++)
                {
                    ret += m_characters[i].Cost;
                }
            }

            return ret;
        }
    }

    public class CharacterTeam
    {
        public Controllers.Character CharacterController => m_character;
        public TBS.Models.CharacterTeam CharacterTeamModel => m_characterTeamModel;
        public int Cost => GetTotalCost();

        public List<Controllers.Item> Items => m_items;
        public List<Controllers.Spell> Spells => m_spells;

        private List<Controllers.Item> m_items;
        private List<Controllers.Spell> m_spells;

        private readonly TBS.Models.CharacterTeam m_characterTeamModel;
        private readonly Controllers.Character m_character;

        public CharacterTeam(TBS.Models.CharacterTeam characterTeamModel)
        {
            m_characterTeamModel = characterTeamModel;
            m_character = Database.Characters.Find(x => x.Id == characterTeamModel.CharacterId);
            m_items = new List<Item>();
            m_spells = new List<Spell>();

            if (characterTeamModel.Items != null)
            {
                for (int i = 0; i < characterTeamModel.Items.Count; i++)
                {
                    m_items.Add(Database.Items.Find(x => x.Id == characterTeamModel.Items[i]));
                }
            }

            if (characterTeamModel.Spells != null)
            {
                for (int i = 0; i < characterTeamModel.Spells.Count; i++)
                {
                    m_spells.Add(Database.Spells.Find(x => x.Id == characterTeamModel.Spells[i]));
                }
            }
        }

        public CharacterTeam(Controllers.Character characterController)
        {
            m_characterTeamModel = new TBS.Models.CharacterTeam();
            m_character = characterController;
            m_items = new List<Item>();
            m_spells = new List<Spell>();
        }

        public void AddItem(Controllers.Item item)
        {
            if (m_characterTeamModel.Items == null)
                m_characterTeamModel.Items = new List<string>();
            if (m_items == null)
                m_items = new List<Item>();

            if (m_items.Contains(item))
                return;

            Controllers.Item sameTypefounded = null;
            if ((sameTypefounded = m_items.Find(x => x.Type == item.Type)) != null)
                m_items.Remove(sameTypefounded);

            m_characterTeamModel.Items.Add(item.Id);
            m_items.Add(item);
        }

        public void AddSpell(Controllers.Spell spell)
        {
            if (m_characterTeamModel.Spells == null)
                m_characterTeamModel.Spells = new List<string>();
            if (m_spells == null)
                m_spells = new List<Spell>();

            if (m_spells.Contains(spell))
                return;

            if (m_character.CharacterModel.SpellIds.Contains(spell.Id))
            {
                m_characterTeamModel.Spells.Add(spell.Id);
                m_spells.Add(spell);
            }
        }

        public void RemoveSpell(Controllers.Spell spell)
        {
            m_characterTeamModel.Spells.Remove(spell.Id);
            m_spells.Remove(spell);
        }

        public void RemoveItem(Controllers.Item item)
        {
            m_characterTeamModel.Items.Remove(item.Id);
            m_items.Remove(item);
        }

        private int GetTotalCost()
        {
            int ret = 0;

            if (Items != null)
            {
                for (int i = 0; i < Items.Count; i ++)
                {
                    ret += Items[i].Price;
                }
            }

            if (Spells != null)
            {
                for (int i = 0; i < Spells.Count; i++)
                {
                    ret += Spells[i].Price;
                }
            }

            return ret + m_character.CharacterModel.Price;
        }
    }
}
