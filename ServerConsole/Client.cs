using System;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace Server
{
    public class Client
    {
        public Room Room { get; private set; }
        public TBSEngine.Controllers.Player Player => m_player;
        public SendFunction SendFunction => m_sendFunction;
        public ReceiveFunction ReceiveFunction => m_receiveFunction;

        private TBSEngine.Controllers.Player m_player;

        private readonly TcpClient m_tcpClient;
        private readonly Thread m_mainClientThread;
        private readonly SendFunction m_sendFunction;
        private readonly ReceiveFunction m_receiveFunction;

        public Client (TcpClient tcpClient)
        {
            m_tcpClient = tcpClient;
            m_sendFunction = new SendFunction(this);
            m_receiveFunction = new ReceiveFunction(this);
            Console.WriteLine("Client created, Starting client thread ...");
            m_mainClientThread = new Thread(ListenClient);
            m_mainClientThread.Start();
        }

        // Set room for client
        public void SetRoom(Room room)
        {
            Console.WriteLine("SetRoom : " + room.ToString());

            // if the client is already in a room
            if (Room != null)
            {
                // if only this client is present in the room
                if (Room.ClientsCount <= 1)
                {
                    // we can delete the room
                    ServerManager.Instance.RemoveRoom(Room);
                }
                else
                {
                    // the client leaves the room
                    Room.RemoveClient(this);
                }
            }

            // join the new room
            ServerManager.Instance.AddRoom(room);
            Room = room;
            room.SetClient(this);
        }

        public void Leave()
        {
            m_mainClientThread.Abort();
            ServerManager.Instance.OnClientLeave(this);
        }

        // Liste client sockets
        private void ListenClient()
        {
            Console.WriteLine("Thread started !");

            while (true)
            {
                if (!m_tcpClient.Connected)
                    Leave();
                else
                {
                    byte[] bytesFrom = new byte[10025];

                    try
                    {
                        NetworkStream networkStream = m_tcpClient.GetStream();
                        networkStream.Read(bytesFrom, 0, 10025);
                        string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                        dataFromClient = dataFromClient.Split('\0')[0];
                        MessageReceivedProcess(TBSEngine.SocketMessage.Parse(dataFromClient));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        // processing of received messages
        public void MessageReceivedProcess(TBSEngine.SocketMessage message)
        {
            Console.WriteLine("Message received : " + message.Message);

            MethodInfo theMethod = typeof(ReceiveFunction).GetMethod(message.FunctionMessage.functionName);
            theMethod.Invoke(m_receiveFunction, message.FunctionMessage.parameters?.ToArray());
        }

        // called when the client identifies himself
        public void ConnexionPlayer(TBSEngine.Controllers.Player player)
        {
            Console.WriteLine(player.Name + " joined");

            m_player = player;
            SetRoom(new PrincipalMenuRoom());
        }

        // send a packet to the client
        public void SendMessage(TBSEngine.SocketMessage message)
        {
            try
            {
                NetworkStream networkStream = m_tcpClient.GetStream();
                byte[] bytesSend = Encoding.ASCII.GetBytes(message.Message); 
                networkStream.Write(bytesSend, 0, bytesSend.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
