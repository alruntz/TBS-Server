using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBSEngine.Controllers
{
    public class Item
    {
        public string Id => m_item.Id;
        public string Name => m_item.Name;
        public int Price => m_item.Price;
        public int MovementPoints => m_item.MovementPoints;
        public int ActionPoints => m_item.ActionPoints;
        public int LifePoints => m_item.LifePoints;
        public int DamagePoints => m_item.DamagePoints;
        public int ArmorPoints => m_item.ArmorPoints;
        public int VisionPoints => m_item.VisionPoints;
        public int InitiativePoints => m_item.InitiativePoints;

        public string Type => m_item.Type;

        public TBS.Models.Item ItemModel => m_item;

        private readonly TBS.Models.Item m_item;

        public Item(TBS.Models.Item item)
        {
            m_item = item;
        }
    }
}
