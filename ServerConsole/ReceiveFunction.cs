using System;

using TBSEngine;
using FS.GridSystem;

namespace Server
{
    public class ReceiveFunction : IServerFunctionReceive
    {
        private readonly Client m_client;

        public ReceiveFunction(Client client)
        {
            m_client = client;
        }

        public void ConnexionUser(string username, string password)
        {
            Console.WriteLine("Function called : ConnexionPlayer");

            TBSEngine.Controllers.Player player = null;

            string token = null;
            if ((token = TBS.APICore.APIManager.Instance.GetTokenSync(username, password)) == null)
            {
                //Console.WriteLine("Error : player " + username + " not found !");
                //m_client.SendFunction.Error("No player founded with this playerName !");
                m_client.SendFunction.ConnexionUser("null");
                return;
            }

            // If Player credentials exist
            player = TBSEngine.Database.Players.Find(x => x.Name == username);
            m_client.ConnexionPlayer(player);
            m_client.SendFunction.ConnexionUser(token);
        }

        public void SearchBattle()
        {
            Console.WriteLine("Function called : SearchBattle");

            // If Client is in Principal Menu Room
            if (m_client.Room != null && m_client.Room.GetType() == typeof(PrincipalMenuRoom))
            {
                ((PrincipalMenuRoom)m_client.Room).ReceiveSearchBattle();
            }
            else
            {
                Console.WriteLine("Error : client isn't in PrincpalMenuRoom");
            }
        }

        public void LaunchSpell(string spellID, string position)
        {
            if (m_client.Room.GetType() == typeof(BattleRoom))
            {
                BattleRoom battleRoom = (BattleRoom)m_client.Room;
                if (battleRoom.IsPlayerTurn(m_client))
                {
                    GridPosition positionParse = new GridPosition(position);
                    if (battleRoom.Battle.LaunchSpell(
                        battleRoom.Battle.ActualCharacter,
                        battleRoom.Battle.ActualCharacter.Spells.Find(x => x.Id == spellID),
                        battleRoom.Battle.Grid.Rows[positionParse.x, positionParse.y]))
                    {
                        battleRoom.GetOtherClient(m_client).SendFunction.LaunchSpell(spellID, position);
                        if (!battleRoom.Battle.IsStarted)
                        {
                            battleRoom.EndGame();
                        }
                    }
                }
            }
        }

        public void MoveCharacter(string position)
        {
            if (m_client.Room.GetType() == typeof(BattleRoom))
            {
                BattleRoom battleRoom = (BattleRoom)m_client.Room;
                if (battleRoom.IsPlayerTurn(m_client))
                {
                    GridPosition positionParse = new GridPosition(position);
                    if (battleRoom.Battle.MoveCharacter(battleRoom.Battle.ActualCharacter, battleRoom.Battle.Grid.Rows[positionParse.x, positionParse.y]))
                    {
                        battleRoom.GetOtherClient(m_client).SendFunction.MoveCharacter(position);
                    }
                }
            }
        }

        public void NextTurn()
        {
            if (m_client.Room.GetType() == typeof(BattleRoom))
            {
                BattleRoom battleRoom = (BattleRoom)m_client.Room;
                if (battleRoom.IsPlayerTurn(m_client))
                {
                    battleRoom.Battle.NextCharacterTurn();
                    Client other = battleRoom.GetOtherClient(m_client);

                    other.SendFunction.NextTurn();
                }
            }
        }
    }
}
