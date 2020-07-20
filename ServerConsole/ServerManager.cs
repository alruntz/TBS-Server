using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Linq;

namespace Server
{
    public class ServerManager
    {
        public static ServerManager Instance => m_instance ?? (m_instance = new ServerManager());

        private List<Client> m_clients;
        private List<Room> m_rooms;

        private TcpListener m_tcpListener;
        private int m_port;
        private bool m_started;

        private static ServerManager m_instance;


        // Start and initialize server
        public void StartServer(int port)
        {
            Console.WriteLine("Server starting at " + port + "port ...");
            m_port = port;
            m_tcpListener = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), port);
            m_tcpListener.Start();
            m_started = true;
            m_clients = new List<Client>();
            Console.WriteLine("Server started !");
            ServerUpdate();
        }

        // Add a new room
        public void AddRoom(Room room)
        {
            if (m_rooms == null)
                m_rooms = new List<Room>();
            m_rooms.Add(room);
        }

        public void RemoveRoom(Room room)
        {
            if (m_rooms != null && m_rooms.Contains(room))
            {
                Console.WriteLine(room.ToString() + " removed");
                m_rooms.Remove(room);
            }
        }

        public SearchBattleRoom GetSearchBattleRoom()
        {
            Room ret = null;
            for (int i = 0; i < m_rooms.Count; i++)
            {
                if (m_rooms[i].GetType() == typeof(SearchBattleRoom))
                    ret = m_rooms[i];
            }
            return ret != null ? (SearchBattleRoom)ret : null;
        }

        private void ServerUpdate()
        {
            TcpClient clientConnected = default(TcpClient);
            while (m_started)
            {
                clientConnected = m_tcpListener.AcceptTcpClient();
                Console.WriteLine("Client joined server");
                OnClientJoin(clientConnected);
            }
        }

        private void OnClientJoin(TcpClient tcpClient)
        {
            Client client = new Client(tcpClient);
            m_clients.Add(client);
            client.SendFunction.Hello();
        }

        public void OnClientLeave(Client client)
        {
            m_rooms.Remove(client.Room);
            m_clients.Remove(client);
        }
    }
}
