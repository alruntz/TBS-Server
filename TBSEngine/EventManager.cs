using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FS.GridSystem;

namespace TBSEngine
{
    public class EventManager
    {
        public class EventMessage
        {
            public enum MessageType
            {
                Success,
                Error
            }

            public MessageType Type => m_type;
            public string Body => m_body;

            private readonly MessageType m_type;
            private readonly string m_body;

            public EventMessage(EventMessage.MessageType type, string body)
            {
                m_type = type;
                m_body = body;
            }
        }

        public class EventMessageHitDamages : EventMessage
        {
            public int Damages => m_damages;
            public BattleEntities.Character Launcher => m_launcher;
            public BattleEntities.Character Target => m_target;

            private readonly int m_damages;
            private readonly BattleEntities.Character m_launcher;
            private readonly BattleEntities.Character m_target;

            public EventMessageHitDamages(EventMessage.MessageType type, string body, int damages, BattleEntities.Character launcher, BattleEntities.Character target)
                : base(type, body)
            {
                m_damages = damages;
                m_launcher = launcher;
                m_target = target;
            }
        }   

        public static event DCharacterHitDamages OnCharacterHitDamages;
        public static event DCharacterDead OnCharacterDead;
        public static event DCharacterMove OnCharacterMove;
        public static event DEndGame OnEndGame;
        public static event DNextTurn OnNextTurn;
        public static event DNextCharacterTurn OnNextCharacterTurn;
        public static event DLaunchSpell OnLaunchSpell;

        public delegate void DCharacterHitDamages(EventMessageHitDamages eventMessage);
        public delegate void DCharacterDead(BattleEntities.Character character);
        public delegate void DCharacterMove(BattleEntities.Character character, Grid.Row destination);
        public delegate void DEndGame(BattleEntities.Player playerWinner);
        public delegate void DNextTurn();
        public delegate void DNextCharacterTurn();
        public delegate void DLaunchSpell(BattleEntities.Spell spell, List<Grid.Row> rangeAOE);

        public static void CharacterHitDamages(EventMessageHitDamages eventMessage)
        {
            OnCharacterHitDamages?.Invoke(eventMessage);
        }

        public static void CharacterDead(BattleEntities.Character character)
        {
            OnCharacterDead?.Invoke(character);
        }

        public static void LaunchSpell(BattleEntities.Spell spell, List<Grid.Row> rangeAOE)
        {
            OnLaunchSpell?.Invoke(spell, rangeAOE);
        }

        public static void CharacterMove(BattleEntities.Character character, Grid.Row destination)
        {
            OnCharacterMove?.Invoke(character, destination);
        }

        public static void EndGame(BattleEntities.Player playerWinner)
        {
            OnEndGame?.Invoke(playerWinner);
        }

        public static void NextTurn()
        {
            OnNextTurn?.Invoke();
        }

        public static void NextCharacterTurn()
        {
            OnNextCharacterTurn?.Invoke();
        }
    }
}