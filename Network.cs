using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace ServerApplication
{
    class Network
    {
        public TcpListener ServerSocket;
        public static Network instance = new Network();
        public static Client[] Clients = new Client[100];

        public delegate void PacketHandler(int _fromClient, Packet _packet);

        public static Dictionary<int, PacketHandler> PacketHandlers;
        public void ServerStart()
        {
            InitializeServerData();
            ServerSocket = new TcpListener(IPAddress.Any, 5500);
            ServerSocket.Start();
            ServerSocket.BeginAcceptTcpClient(OnClientConnect, null);
            Console.WriteLine("Server has Started with IP: " + IPAddress.Any + " Port: 5500");
        }

        void OnClientConnect(IAsyncResult result)
        {
            TcpClient client = ServerSocket.EndAcceptTcpClient(result);
            client.NoDelay = false;
            ServerSocket.BeginAcceptTcpClient(OnClientConnect, null);
            for (int i = 1; i < 100; i++)
            {
                if (Clients[i].Socket == null)
                {
                    Clients[i].Socket = client;
                    Clients[i].Index = i;
                    Clients[i].IP = client.Client.RemoteEndPoint.ToString();
                    Clients[i].Start();
                    Console.WriteLine("[Index: " + i  +"] Client Connect from: " + Clients[i].IP);
                    ServerSend.Welcome(i, "Welcome to the Server!");
                    return;
                }
            }
        }

        private static void InitializeServerData()
        {
            for (int i = 1; i < 100; i++)
            {
                Clients[i] = new Client();
            }

            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.registerReceived, ServerHandle.RegisterReceived},
                { (int)ClientPackets.loginReceived, ServerHandle.LoginReceived},
                { (int)ClientPackets.getaccountReceived, ServerHandle.GetAccReceived},
                { (int)ClientPackets.createaccReceived, ServerHandle.CreateAccReceived}
            };
            Console.WriteLine("Install Packet");
        }
    }
}