using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ServerApplication
{
    public class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _msg = _packet.ReadString();
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine("Error Player");
            }
        }

        public static void RegisterReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string Username = _packet.ReadString();
            string Password = _packet.ReadString();
            Console.WriteLine("Username: " + Username + " Password: " + Password);
            int result = 1;
            if (!MySql.mysql.CheckUsername(Username))
            {
                result = 0;
                MySql.mysql.CreateUser(Username,Password, "a@gmail.com");
            }
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine("Error Player");
            }
                ServerSend.Register(_clientIdCheck, result);
        }

        public static void LoginReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string Username = _packet.ReadString();
            string Password = _packet.ReadString();
            int result = 2, Uin = 0;
            if (MySql.mysql.CheckUsername(Username))
            {
                Uin = MySql.mysql.GetUin(Username);
                if (MySql.mysql.CheckPass(Username,Password))result = 0;
                else result = 1;
            }
            ServerSend.Login(_fromClient, result, Uin);
        }

        public static void GetAccReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            int Uin = _packet.ReadInt();
            int Gold = 0, Level = 0;float MaxExp =0, Exp = 0;
            string username = "";
            int result = 0;
            if (MySql.mysql.checkAcc(Uin))
            {
                result = 1;
                ServerSend.Getacc(_fromClient, result, Uin, username, Level, Exp,Gold,MaxExp);
                
            }
            else
            {
                MySql.mysql.GetInfomationUser(_fromClient,Uin, out username,out Level,out Exp, out Gold);
                MaxExp = MySql.mysql.GetMaxExp(Level);
                ServerSend.Getacc(_fromClient, result, Uin, username, Level, Exp,Gold, MaxExp);
            }
            
        }

        public static void CreateAccReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string Username = _packet.ReadString();
            int Uin = _packet.ReadInt();
            int result = 1;
            if (MySql.mysql.CheckUser(Username))
            {
                result = 0;
                MySql.mysql.CreateAcc(Uin, Username);
                MySql.mysql.createaquarium(Uin, 1);
            }
            ServerSend.Regacc(_fromClient, result);
        }

        public static void LevelUpReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            int Uin = _packet.ReadInt();
            int Level = _packet.ReadInt();
            float Exp = _packet.ReadFloat();
            float MaxExp = MySql.mysql.GetMaxExp(Level);
            MySql.mysql.LevelUp(Uin,Level, Exp);
            ServerSend.LevelUp(_fromClient,Exp,MaxExp,Level);
        }

        public static void GainExpReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            int Uin = _packet.ReadInt();
            float Exp = _packet.ReadFloat();
            MySql.mysql.GainExp(Uin, Exp);
            ServerSend.GainExp(_fromClient,Exp);
        }

        public static void GetShopReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            ServerSend.GetShop(_fromClient);
        }

        public static void GetAquariumReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            int Uin = _packet.ReadInt();
            int numaqua, numfish = 0;
            Aquarium[] aquarium = new Aquarium[5];
            Fish[] fishs = new Fish[] { };
            MySql.mysql.getaquarium(Uin, out aquarium, out numaqua);
            for (int i = 1; i <= numaqua; i++)
            {
                MySql.mysql.getfish(i, out fishs, out numfish);
            }
            ServerSend.GetAquarium(_fromClient, aquarium, numaqua, fishs, numfish);
        }
        
        public static void BuyFish(int _fromClient,Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            Fish fish = new Fish();
            int Uin = _packet.ReadInt();
            fish.FishID = _packet.ReadInt();
            fish.name = _packet.ReadString();
            fish.Gender = _packet.ReadInt();
            fish.IDAqua = _packet.ReadInt();
            int cost = _packet.ReadInt();
            fish.exp = _packet.ReadFloat();
            fish.gold = _packet.ReadInt();
            int ID = 0;
            
            if (MySql.mysql.checkcost(cost, Uin))
            {
                MySql.mysql.addFish(fish.FishID, fish.IDAqua, fish.name, fish.Gender, fish.gold, fish.exp);
                MySql.mysql.GoldRemove(cost, Uin);
                ServerSend.buyfish(_fromClient, 1, fish, ID);
            }
            ServerSend.buyfish(_fromClient, 0, fish, ID);
            
        }

        public static void Eatting(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            int ID = _packet.ReadInt();
            float Food = _packet.ReadInt();
        }
        
        
    }

    public class Aquarium
    {
        public int ID = 0 ;
        public int Slot = 0;
        public int MaxFish = 0;
        public int CurFish = 0;
    }

    public class Fish
    {
        public int ID;
        public int FishID;
        public int IDAqua;
        public string name;
        public int Level;
        public float Food;
        public float Grow;
        public int Gender;
        public int gold;
        public float exp;
    }
    
    
}