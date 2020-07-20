using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TBSEngine;

namespace Server
{
   // the functions to send to the client

    public class SendFunction : IServerFunctionSend
    {
        private readonly Client m_client;

        public SendFunction(Client client)
        {
            m_client = client;
        }

        public void Error(string message)
        {
            m_client.SendMessage(new SocketMessage(new SocketMessage.FunctionSocketMessage("Error", new List<string> { message })));
        }

        public void Hello()
        {
            m_client.SendMessage(new SocketMessage(new SocketMessage.FunctionSocketMessage("Hello", null)));
        }

        public void ConnexionUser(string token)
        {
            m_client.SendMessage(new SocketMessage(new SocketMessage.FunctionSocketMessage("ConnexionUser", new List<string> { token })));
        }

        public void LaunchBattle(string playerNames, string mapId, string characterPositions)
        {
            m_client.SendMessage(new SocketMessage(new SocketMessage.FunctionSocketMessage("LaunchBattle", new List<string> { playerNames, mapId, characterPositions  })));
        }

        public void LaunchSpell(string spellID, string position)
        {
            m_client.SendMessage(new SocketMessage(new SocketMessage.FunctionSocketMessage("LaunchSpell", new List<string> { spellID, position })));
        }

        public void MoveCharacter(string position)
        {
            m_client.SendMessage(new SocketMessage(new SocketMessage.FunctionSocketMessage("MoveCharacter", new List<string> { position })));
        }

        public void NextTurn()
        {
            m_client.SendMessage(new SocketMessage(new SocketMessage.FunctionSocketMessage("NextTurn", null)));
        }
    }
}
