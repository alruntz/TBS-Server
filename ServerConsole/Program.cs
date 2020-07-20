using System;
using System.Collections.Generic;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {

            if (TBSEngine.Database.Init("Flutes", "pwd").Result != null)
            {
                Console.WriteLine("Connexion database success !");
                if (TBSEngine.Database.LoadAll())
                {
                    Console.WriteLine("Load database contents success !");
                    ServerManager.Instance.StartServer(7755);
                }
            }
            else
                Console.WriteLine("Error Database Connexion");
            
        }
    }
}
