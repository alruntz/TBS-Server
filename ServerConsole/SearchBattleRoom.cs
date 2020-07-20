using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class SearchBattleRoom : Room
    {
        public SearchBattleRoom() : base()
        {

        }

        public void PrepareLaunchBattle(Client otherClient)
        {
            Console.WriteLine("Prepare launch battle ...");
            BattleRoom battleRoom = new BattleRoom();
            m_clients[0].SetRoom(battleRoom);
            otherClient.SetRoom(battleRoom);
            battleRoom.SendLaunchBattle();
        }
    }
}
