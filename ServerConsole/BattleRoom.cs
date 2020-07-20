using System;
using System.Collections.Generic;
using System.Text;

using FS.GridSystem;

using TBSEngine;

namespace Server
{
    public class BattleRoom : Room
    {
        public List<TBSEngine.Controllers.Player> Players => new List<TBSEngine.Controllers.Player>() { m_clients[0].Player, m_clients[1].Player };
        public Battle Battle => m_battle;

        private FS.GridSystem.Grid m_grid;
        private TBS.Models.Map m_map;
        private Battle m_battle;

        public BattleRoom() : base()
        {
        }

        public bool IsPlayerTurn(Client client)
        {
            return m_battle.ActualPlayer.PlayerController == client.Player;
        }

        public Client GetOtherClient(Client myClient)
        {
            return m_clients.Find(x => x != myClient);
        }

        public void SendLaunchBattle()
        {
            Console.WriteLine("Launch battle !");

            m_grid = new FS.GridSystem.Grid();
            m_map = TBSEngine.Database.Maps[1];
            m_grid.CreateGrid(new GridPosition(m_map.Width, m_map.Length), TBSEngine.GridHelper.MapToGrid(m_map));

            m_battle = new Battle(Players, m_grid, true);

            string playerNames = m_clients[0].Player.Name + "|" + m_clients[1].Player.Name;
            m_clients[0].SendFunction.LaunchBattle(playerNames, m_map.Id, m_battle.GetCharactersPositionString());
            m_clients[1].SendFunction.LaunchBattle(playerNames, m_map.Id, m_battle.GetCharactersPositionString());

            m_battle.OnTimerTurnFinish += OnTurnTimerFinish;
            m_battle.StartTimerTurnThread();
        }

        public void OnTurnTimerFinish()
        {
            // m_clients[0].SendFunction.NextTurn();
            // m_clients[0].ReceiveFunction.NextTurn();
            // m_clients[1].SendFunction.NextTurn();
            // m_clients[1].ReceiveFunction.NextTurn();
            Console.WriteLine("__OnTurnTimerFinish__");
            //if (IsPlayerTurn(m_clients[0]))
                m_clients[0].SendFunction.NextTurn();
            //else
                m_clients[1].SendFunction.NextTurn();

            m_battle.NextCharacterTurn();
        }

        public void EndGame()
        {
            Client a = m_clients[0];
            Client b = m_clients[1];

            a.SetRoom(new PrincipalMenuRoom());
            b.SetRoom(new PrincipalMenuRoom());
        }
    }
}
