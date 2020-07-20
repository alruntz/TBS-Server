using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FS.GridSystem;

namespace TBSEngine.BattleEntities
{
    public class Range
    {
        public int StartDistance => m_range.StartDistance;
        public BattleEntities.Character Character => m_character;
       
        private readonly Controllers.Spell m_spell;
        private readonly Controllers.Range m_range;
        private readonly BattleEntities.Character m_character;

        public Range(Controllers.Range range, Controllers.Spell spell, BattleEntities.Character character)
        {
            m_range = range;
            m_spell = spell;
            m_character = character;
        }
    }

    public class SpellRange : Range
    {
        public Controllers.SpellRange.RangeType Type => m_range.Type;
        public bool Boostable => m_range.Boostable;
        public int  Size => m_range.Boostable ? m_range.Size + Character.VisionPoints : m_range.Size;
        public Grid.Range GridRange => new Grid.Range(Size, (Grid.Range.RangeType)Enum.Parse(typeof(Grid.Range.RangeType), Type.ToString()));

        private readonly Controllers.SpellRange m_range;
        private readonly Controllers.Spell m_spell;

        public SpellRange(Controllers.SpellRange range, Controllers.Spell spell, BattleEntities.Character character) : base(range, spell, character)
        {
            m_range = range;
            m_spell = spell;
        }
    }

    public class EffectRange : Range
    {
        public Controllers.EffectRange.RangeType Type => m_range.Type;
        public int Size => m_range.Size;
        public Grid.Range GridRange => new Grid.Range(Size, (Grid.Range.RangeType)Enum.Parse(typeof(Grid.Range.RangeType), Type.ToString()));

        private readonly Controllers.EffectRange m_range;
        private readonly Controllers.Spell m_spell;

        public EffectRange(Controllers.EffectRange range, Controllers.Spell spell, BattleEntities.Character character) : base(range, spell, character)
        {
            m_range = range;
            m_spell = spell;
        }
    }

    public class Spell
    {
        public string Id => m_spellController.Id;
        public string Name => m_spellController.Name;
        public int Damages => m_spellController.Damages + m_characterBattle.DamagePoints;
        public int ActionPointsCost => m_spellController.ActionPointsCost;
        public SpellRange SpellRange => m_spellRange;
        public EffectRange EffectRange => m_effectRange;

        public bool Usable => ActionPointsCost <= m_characterBattle.ActionPoints;

        public BattleEntities.Character CharacterBattle => m_characterBattle;
        public Controllers.Spell SpellController => m_spellController;

        private readonly Controllers.Spell m_spellController;
        private readonly BattleEntities.Character m_characterBattle;
        private readonly SpellRange m_spellRange;
        private readonly EffectRange m_effectRange;

        public Spell(Controllers.Spell spellController, BattleEntities.Character characterBattle)
        {
            m_spellController = spellController;
            m_characterBattle = characterBattle;
            m_spellRange = new SpellRange(spellController.SpellRange, spellController, characterBattle);
            m_effectRange = new EffectRange(SpellController.EffectRange, spellController, characterBattle);
        }
    }
}
