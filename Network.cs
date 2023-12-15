using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Net.Configuration;
using System.Text;
using Microsoft.DocAsCode.YamlSerialization;
using Newtonsoft.Json;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace ServerApplication
{
    class Network
    {
        public TcpListener ServerSocket;
        public static Network instance = new Network();
        public static Client[] Clients = new Client[100];
        public static List<MaxExp> MaxExps = new List<MaxExp>();
        public static List<Shop> Shops = new List<Shop>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);

        public static Dictionary<int, PacketHandler> PacketHandlers;
        public void ServerStart()
        {
            Console.OutputEncoding = Encoding.UTF8;
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

        public class MaxExp
        {
            public float Exp { get; set; }

            public override string ToString()
            {
                return $"{this.Exp.ToString()}";
            }
        }
        
        public class Shop
        {
            public int ItemId { get; set; }
            public int Type { get; set; }
            public string Name { get; set; }
            public int Level { get; set; }
            public float Time { get; set; }
            public float Exp { get; set; }
            public int Gold { get; set; }
            public int Cost { get; set; }
            
            public override string ToString()
            {
                return $"[{ItemId}]{Name} Type: {Type} Level: {Level} Time: {Time} Exp: {Exp} Gold: {Gold} Cost: {Cost}";
            }
        }
        
        private static void InitializeServerData()
        {
            for (int i = 1; i < 100; i++)
            {
                Clients[i] = new Client();
            }
            
            
            try
            {
                var deserializer = new DeserializerBuilder()
                    .Build();
                using var sr = File.OpenText("Exp.yml");
                MaxExps = deserializer.Deserialize<List<MaxExp>>(sr);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Exp.yml");
                throw;
            }

            try
            {
                var deserializer = new DeserializerBuilder()
                    .Build();
                using var sr = File.OpenText("Shop.yml");
                Shops = deserializer.Deserialize<List<Shop>>(sr);
                

            }
            catch (Exception e)
            {
                Console.WriteLine("Error Shop.yml");
                throw;
            }
            
            

        PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.registerReceived, ServerHandle.RegisterReceived},
                { (int)ClientPackets.loginReceived, ServerHandle.LoginReceived},
                { (int)ClientPackets.getaccountReceived, ServerHandle.GetAccReceived},
                { (int)ClientPackets.createaccReceived, ServerHandle.CreateAccReceived},
                { (int)ClientPackets.levelupReceived, ServerHandle.LevelUpReceived},
                { (int)ClientPackets.gainexpReceived, ServerHandle.GainExpReceived},
                { (int)ClientPackets.getshopReceived, ServerHandle.GetShopReceived},
                { (int)ClientPackets.getaquariumReceived, ServerHandle.GetAquariumReceived},
                { (int)ClientPackets.buyfish, ServerHandle.BuyFish},
                { (int)ClientPackets.eatting, ServerHandle.Eatting}
            };
            
            Console.WriteLine("Install Packet");
        }
    }
    
}