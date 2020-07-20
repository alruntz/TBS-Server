using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using FS.GridSystem;
using System.Reflection;

namespace TBSEngine
{
    public class Battle
    {
        public List<BattleEntities.Player> Players => m_players;
        public List<BattleEntities.Character> Characters => m_characters;
        public List<BattleEntities.Character> CharactersDead => m_charactersDead;
        public int Turn => m_turn;
        public int CharacterTurn => m_characterTurn;
        public bool IsStarted => m_isStarted;
        public Grid Grid => m_grid;

        public BattleEntities.Character ActualCharacter { get { return m_characters[m_characterTurn % m_characters.Count]; } }
        public BattleEntities.Player ActualPlayer { get { return ActualCharacter.PlayerBattle; } }

        private int m_turn;
        private int m_characterTurn;
        private bool m_isStarted;
        private List<BattleEntities.Character> m_charactersDead;

        private readonly List<BattleEntities.Player> m_players;
        private readonly List<BattleEntities.Character> m_characters;
        private readonly Grid m_grid;

        private bool m_timerEnabled;
        public delegate void DTimerTurnFinish();
        public event DTimerTurnFinish OnTimerTurnFinish;
        private Thread m_timerTurnThread;

        
        public Battle (List<Controllers.Player> players, Grid grid, bool timer)
        {
            m_grid = grid;
            m_players = new List<BattleEntities.Player>();
            m_characters = new List<BattleEntities.Character>();
            m_charactersDead = new List<BattleEntities.Character>();
            m_isStarted = true;
            m_timerEnabled = timer;
            m_timerTurnThread = null;
            

            for (int i = 0; i < players.Count; i++)
            {
                m_players.Add(new BattleEntities.Player(players[i]));
                for (int j = 0; j < m_players[i].CharacterBattles.Count; j++)
                {
                    m_characters.Add(m_players[i].CharacterBattles[j]);
                    m_characters.Last().StartBattle(Grid.GetRandomRow(new List<Grid.Row>()));
                }
            }
        }

        public Battle(List<Controllers.Player> players, Grid grid, string charactersPositions)
        {
            m_grid = grid;
            m_players = new List<BattleEntities.Player>();
            m_characters = new List<BattleEntities.Character>();
            m_charactersDead = new List<BattleEntities.Character>();
            m_isStarted = true;

            List<GridPosition> charactersPositionA = new List<GridPosition>();
            List<GridPosition> charactersPositionB = new List<GridPosition>();

            string[] strCharacters = charactersPositions.Split('|');
            string[] strCharactersA = strCharacters[0].Split('_');
            string[] strCharactersB = strCharacters[1].Split('_');

            for (int i = 0; i < strCharactersA.Length; i++)
                charactersPositionA.Add(new GridPosition(strCharactersA[i]));

            for (int i = 0; i < strCharactersB.Length; i++)
                charactersPositionB.Add(new GridPosition(strCharactersB[i]));


            m_players.Add(new BattleEntities.Player(players[0]));
            m_players.Add(new BattleEntities.Player(players[1]));

            for (int i = 0; i < charactersPositionA.Count; i++)
            {
                m_characters.Add(m_players[0].CharacterBattles[i]);
                m_characters.Last().StartBattle(Grid.Rows[charactersPositionA[i].x, charactersPositionA[i].y]);
            }

            for (int i = 0; i < charactersPositionB.Count; i++)
            {
                m_characters.Add(m_players[1].CharacterBattles[i]);
                m_characters.Last().StartBattle(Grid.Rows[charactersPositionB[i].x, charactersPositionB[i].y]);
            }

            m_characters.OrderBy(x => x.InitiativePoints).Reverse();
        }


        public string GetCharactersPositionString()
        {
            string ret = "";

            for (int i = 0; i < m_players.Count; i++)
            {
                for (int j = 0; j < m_players[i].CharacterBattles.Count; j++)
                {
                    ret += m_players[i].CharacterBattles[j].Row.position.ToString();

                    if (j != m_players[i].CharacterBattles.Count - 1)
                        ret += "_";
                }

                if (i != m_players.Count - 1)
                    ret += "|";
            }

            return ret;
        }

        public void StartTimerTurnThread()
        {
            Console.WriteLine("Start Timer ...");
            if (m_timerEnabled)
            {
                Console.WriteLine("Timer Started");
                m_timerTurnThread = new Thread(StartTimerTurn);
                m_timerTurnThread.Start();
            }
        }

        private void StartTimerTurn()
        {
            Console.WriteLine("New Timer Thread created");
            Thread.Sleep(30000);
            Console.WriteLine("Timer Thread finish");
            if (m_timerTurnThread == Thread.CurrentThread)
                OnTimerTurnFinish.Invoke();
        }

        private void NextTurn()
        {
            m_turn++;
            m_characterTurn = 0;

            EventManager.NextTurn();
        }

        public void NextCharacterTurn()
        {
            ActualCharacter.ResetActionPoints();
            ActualCharacter.ResetMovementPoints();

            if (m_characterTurn + 1 >= m_characters.Count)
                NextTurn();
            else
                m_characterTurn++;

            EventManager.NextCharacterTurn();

            StartTimerTurnThread();
        }

        public bool MoveCharacter(BattleEntities.Character characterBattle, Grid.Row destination)
        {
            if (characterBattle.Move(Grid, destination))
            {
                EventManager.CharacterMove(characterBattle, destination);
                return true;
            }

            return false;
        }

        //don't use bool with client (unity)
        public bool LaunchSpell(BattleEntities.Character characterBattle, BattleEntities.Spell spell, Grid.Row destination)
        {
            List<Grid.Row> range = Grid.GetAOERange(spell.EffectRange.GridRange, destination, characterBattle.Row);
            List<BattleEntities.Character> targets = new List<BattleEntities.Character>();


            if (!characterBattle.LaunchSpell(spell))
            {
                return false;
            }

            EventManager.LaunchSpell(spell, range);

            for (int i = 0; i < range.Count; i++)
            {
                BattleEntities.Character character = null;
                if ((character = Characters.Find(x => x.Row == range[i])) != null)
                    targets.Add(character);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].HitDamages(spell);
                if (targets[i].IsDead)
                {
                    m_charactersDead.Add(m_characters.Find(x => x == targets[i]));
                    m_characters.Remove(targets[i]);

                    if (targets[i] == ActualCharacter)
                        NextCharacterTurn();
                }
            }

            if (PlayerLose(m_players[0]))
            {
                EndGame(m_players[1]);
            }

            else if (PlayerLose(m_players[1]))
            {
                EndGame(m_players[0]);
            }

            return true;
        }

        private bool PlayerLose(BattleEntities.Player player)
        {
            int count = 0;
            for (int i = 0; i < player.CharacterBattles.Count; i++)
            {
                if (player.CharacterBattles[i].IsDead)
                    count++;
            }
            if (count == player.CharacterBattles.Count)
                return true;

            return false;
        }

        private void EndGame(BattleEntities.Player playerWinner)
        {
            EventManager.EndGame(playerWinner);
            m_isStarted = false;
        }
    }
}
