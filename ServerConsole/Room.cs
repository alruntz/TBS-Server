using System;
using System.Collections.Generic;
using System.Text;

using TBSEngine;

namespace Server
{
    public class Room
    {
        public int ClientsCount => m_clients == null ? 0 : m_clients.Count;

        protected List<Client> m_clients;

        public Room()
        {
            m_clients = new List<Client>();
        }

        public void SetClient(Client client)
        {
            m_clients.Add(client);
            Console.WriteLine(client.Player.Name + " joined " + this.ToString());
        }

        public void RemoveClient(Client client)
        {
            m_clients.Remove(client);
            Console.WriteLine(client.Player.Name + " leave " + this.ToString());
        }

    }
}
