using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class PrincipalMenuRoom : Room
    {
        public PrincipalMenuRoom() : base()
        {

        }

        public void ReceiveSearchBattle()
        {
            if (m_clients != null)
            {
                SearchBattleRoom searchBattleRoom;
                if (( searchBattleRoom = ServerManager.Instance.GetSearchBattleRoom()) != null)
                {
                    searchBattleRoom.PrepareLaunchBattle(m_clients[0]);
                }
                else
                {
                    m_clients[0].SetRoom(new SearchBattleRoom());
                }
            }
            else
            {
                Console.WriteLine("Error : clients list is null in PrincipalMenuRoom");
            }
        }
    }
}
