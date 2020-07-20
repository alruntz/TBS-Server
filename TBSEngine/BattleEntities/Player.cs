using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBSEngine.BattleEntities
{
    public class Player
    {
        public List<BattleEntities.Character> CharacterBattles => m_characterBattles;
        public Controllers.Player PlayerController => m_playerController;

        private readonly Controllers.Player m_playerController;
        private readonly List<BattleEntities.Character> m_characterBattles;

        public Player(Controllers.Player playerController)
        {
            m_playerController = playerController;
            m_characterBattles = new List<BattleEntities.Character>();

            for (int i = 0; i < playerController.Team.Characters.Count; i++)
            {
                m_characterBattles.Add(new BattleEntities.Character(playerController.Team.Characters[i], this));
            }
        }
    }
}
