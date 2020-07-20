using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FS.GridSystem;

namespace TBSEngine.BattleEntities
{
    public class Character
    {

        public string Id => m_characterController.Id;
        public string Name => m_characterController.Name;
        public List<Controllers.Item> Items => m_items;

        public List<BattleEntities.Spell> SpellsBase => m_spells;
        public int MovementPointsBase => m_characterController.MovementPoints + m_movementPointsBonus;
        public int ActionPointsBase => m_characterController.ActionPoints + m_actionPointsBonus;
        public int LifePointsBase => m_characterController.LifePoints + m_lifePointsBonus;
        public int DamagePointsBase => m_characterController.DamagePoints + m_damagePointsBonus;
        public int ArmorPointsBase => m_characterController.ArmorPoints + m_armorPointsBonus;
        public int VisionPointsBase => m_characterController.VisionPoints + m_visionPointsBonus;
        public int InitiativePointsBase => m_characterController.InitiativePoints + m_initiativePointsBonus;

        public List<BattleEntities.Spell> Spells => m_spells;
        public int MovementPoints => m_movementPoints + m_movementPointsBonus;
        public int ActionPoints => m_actionPoints + m_actionPointsBonus;
        public int LifePoints => m_lifePoints + m_lifePointsBonus;
        public int DamagePoints => m_damagePoints + m_damagePointsBonus;
        public int ArmorPoints => m_armorPoints + m_armorPointsBonus;
        public int VisionPoints => m_visionPoints + m_visionPointsBonus;
        public int InitiativePoints => m_initiativePoints + m_initiativePointsBonus;

        public Grid.Range GridRangeMovements => new Grid.Range(MovementPointsBase, Grid.Range.RangeType.Default);
        public Controllers.Character CharacterController => m_characterController;
        public BattleEntities.Player PlayerBattle => m_playerBattle;
        public bool IsDead => m_isDead;

        public Grid.Row Row => m_row;
        public bool IsStarted => m_isStarted;

        private int m_movementPoints;
        private int m_actionPoints;
        private int m_lifePoints;
        private int m_damagePoints;
        private int m_armorPoints;
        private int m_visionPoints;
        private int m_initiativePoints;

        private List<Controllers.Item> m_items;

        private int m_movementPointsBonus
        {
            get
            {
                int value = 0;

                if (m_items != null)
                {
                    for (int i = 0; i < m_items.Count; i++)
                    {
                        value += m_items[i].MovementPoints;
                    }
                }

                return value;
            }
        }

        private int m_actionPointsBonus
        {
            get
            {
                int value = 0;

                if (m_items != null)
                {
                    for (int i = 0; i < m_items.Count; i++)
                    {
                        value += m_items[i].ActionPoints;
                    }
                }

                return value;
            }
        }

        private int m_lifePointsBonus
        {
            get
            {
                int value = 0;

                if (m_items != null)
                {
                    for (int i = 0; i < m_items.Count; i++)
                    {
                        value += m_items[i].LifePoints;
                    }
                }

                return value;
            }
        }


        private int m_damagePointsBonus
        {
            get
            {
                int value = 0;

                if (m_items != null)
                {
                    for (int i = 0; i < m_items.Count; i++)
                    {
                        value += m_items[i].DamagePoints;
                    }
                }

                return value;
            }
        }

        private int m_armorPointsBonus
        {
            get
            {
                int value = 0;

                if (m_items != null)
                {
                    for (int i = 0; i < m_items.Count; i++)
                    {
                        value += m_items[i].ArmorPoints;
                    }
                }

                return value;
            }
        }

        private int m_visionPointsBonus
        {
            get
            {
                int value = 0;

                if (m_items != null)
                {
                    for (int i = 0; i < m_items.Count; i++)
                    {
                        value += m_items[i].VisionPoints;
                    }
                }

                return value;
            }
        }

        private int m_initiativePointsBonus
        {
            get
            {
                int value = 0;

                if (m_items != null)
                {
                    for (int i = 0; i < m_items.Count; i++)
                    {
                        value += m_items[i].InitiativePoints;
                    }
                }

                return value;
            }
        }


        private bool m_isStarted;
        private Grid.Row m_row;
        private bool m_isDead;

        private readonly Controllers.Character m_characterController;
        private readonly BattleEntities.Player m_playerBattle;
        private readonly List<BattleEntities.Spell> m_spells;

        public Character(Controllers.CharacterTeam characterTeam, BattleEntities.Player playerBattle = null)
        {
            m_characterController = characterTeam.CharacterController;
            m_playerBattle = playerBattle;
            m_spells = new List<Spell>();

            for (int i = 0; i < m_characterController.SpellControllers.Count; i++)
            {
                m_spells.Add(new BattleEntities.Spell(m_characterController.SpellControllers[i], this));
            }

            m_movementPoints = MovementPointsBase;
            m_actionPoints = ActionPointsBase;
            m_damagePoints = DamagePointsBase;
            m_armorPoints = ArmorPointsBase;
            m_visionPoints = VisionPointsBase;
            m_lifePoints = LifePointsBase;
            m_initiativePoints = InitiativePoints;

            m_items = characterTeam.Items;
        }

        public void StartBattle(Grid.Row row)
        {
            m_row = row;
            m_isStarted = true;
        }

        public bool Move(Grid grid, Grid.Row destination)
        {
            List<Grid.Row> path = new List<Grid.Row>();

            if ((path = Dijkstra.GetBestPath(grid, m_row, destination, MovementPoints)) != null)
            {
                m_row.occuped = false;

                m_row = destination;
                m_row.occuped = true;
                m_movementPoints -= path.Count;
                return true;
            }

            return false;
        }

        public bool LaunchSpell(BattleEntities.Spell spell)
        {
            if (ActionPoints >= spell.ActionPointsCost)
                m_actionPoints -= spell.ActionPointsCost;
            else
                return false;

            return true;
        }

        public void HitDamages(BattleEntities.Spell spell)
        {
            if (LifePoints > spell.Damages)
                m_lifePoints -= spell.Damages;
            else
                Death();

            EventManager.CharacterHitDamages(new EventManager.EventMessageHitDamages(EventManager.EventMessage.MessageType.Success, "", spell.Damages, spell.CharacterBattle, this));
        }

        public void ResetActionPoints()
        {
            m_actionPoints = ActionPointsBase;
        }

        public void ResetMovementPoints()
        {
            m_movementPoints = MovementPointsBase;
        }

        public void Death()
        {
            m_isDead = true;
            EventManager.CharacterDead(this);
        }
    }
}
