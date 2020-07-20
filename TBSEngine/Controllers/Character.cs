using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBSEngine.Controllers
{
    public class Character
    {
        public string Id => m_characterModel.Id;
        public string Name => m_characterModel.Name;
        public List<Controllers.Spell> SpellControllers => m_spells;
        public int MovementPoints => m_characterModel.MovementPoints;
        public int ActionPoints => m_characterModel.ActionPoints;
        public int LifePoints => m_characterModel.LifePoints;
        public int DamagePoints => m_characterModel.DamagePoints;
        public int ArmorPoints => m_characterModel.ArmorPoints;
        public int VisionPoints => m_characterModel.VisionPoints;
        public int InitiativePoints => m_characterModel.InitiativePoints;
        public int Price => m_characterModel.Price;

        public TBS.Models.Character CharacterModel => m_characterModel;

        private List<Controllers.Spell> m_spells;

        private TBS.Models.Character m_characterModel;

        public Character(TBS.Models.Character characterModel)
        {
            m_characterModel = characterModel;
            RefreshSpells();
        }

        public bool RefreshSpells()
        {
            if (Database.Spells == null)
                return false;

            m_spells = new List<Controllers.Spell>();

            for (int i = 0; i < m_characterModel.SpellIds.Count; i++)
            {
                m_spells.Add(Database.Spells.Find(x => x.Id == m_characterModel.SpellIds[i]));
            }

            return true;
        }
    }
}
