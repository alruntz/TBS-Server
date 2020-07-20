using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBSEngine
{
    public interface IServerFunctionSend
    {
        void Hello();
        void Error(string message);

        void ConnexionUser(string token);

        void LaunchBattle(string playerNames, string mapId, string characterPositions);

        void LaunchSpell(string spellID, string position);
        void MoveCharacter(string position);
        void NextTurn();
    }
}
