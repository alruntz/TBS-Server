using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBSEngine.Controllers
{

    public class Range
    {
        public int Size => m_range.Size;
        public int StartDistance => m_range.StartDistance;

        private TBS.Models.Range m_range;

        public Range(TBS.Models.Range range)
        {
            m_range = range;
        }
    }

    public class SpellRange : Range
    {
        public enum RangeType
        {
            Default,
            Cross
        }

        public RangeType Type => (RangeType)Enum.Parse(typeof(RangeType), m_range.Type);
        public bool Boostable => m_range.Boostable;

        private TBS.Models.SpellRange m_range;

        public SpellRange(TBS.Models.SpellRange range) : base(range)
        {
            m_range = range;
        }
    }

    public class EffectRange : Range
    {
        public enum RangeType
        {
            Default,
            Cross,
            HorizontallyLine,
            VerticallyLine
        }

        public RangeType Type => (RangeType)Enum.Parse(typeof(RangeType), m_range.Type);

        private TBS.Models.EffectRange m_range;

        public EffectRange(TBS.Models.EffectRange range) : base(range)
        {
            m_range = range;
        }
    }

    public class Spell
    {
        public string Id => m_spellModel.Id;
        public string Name => m_spellModel.Name;
        public int Damages => m_spellModel.Damages;
        public int ActionPointsCost => m_spellModel.ActionPointsCost;
        public int Price => m_spellModel.Price;
        public SpellRange SpellRange => m_spellRange;
        public EffectRange EffectRange => m_effectRange;

        public TBS.Models.Spell SpellModel => m_spellModel;

        private readonly TBS.Models.Spell m_spellModel;
        private readonly SpellRange m_spellRange;
        private readonly EffectRange m_effectRange;

        public Spell (TBS.Models.Spell spellModel)
        {
            m_spellModel = spellModel;
            m_spellRange = new SpellRange(spellModel.SpellRange);
            m_effectRange = new EffectRange(spellModel.EffectRange);
        }
    }
}
