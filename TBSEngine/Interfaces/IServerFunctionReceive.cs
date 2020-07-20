using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBSEngine
{
    public interface IServerFunctionReceive
    {
        void ConnexionUser(string username, string password);
        void SearchBattle();

        void LaunchSpell(string spellID, string position);
        void MoveCharacter(string position);
        void NextTurn();
    }
}
