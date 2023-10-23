using System;
using MySql.Data.MySqlClient;

namespace ServerApplication
{
    public class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _msg = _packet.ReadString();
            
            Console.WriteLine($"Connect successfully and send msg: {_msg}");
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
            int result = 0;
            if (MySql.mysql.CheckUsername(Username))
            {
                result = 1;
            }
            else
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
                if (MySql.mysql.CheckPass(Username,Password))
                {
                    result = 0;
                }
                else
                {
                    result = 1;
                }
                
            }
            else
            {
                result = 2;
            }

            
            ServerSend.Login(_fromClient, result, Uin);
        }

        public static void GetAccReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            int Uin = _packet.ReadInt();
            int result = 0;
            if (!MySql.mysql.checkAcc(Uin))
            {
                result = 0;
            }
            else
            {
                result = 1;
            }
            ServerSend.Getacc(_fromClient, result);
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
            }
            else result = 1;
            ServerSend.Regacc(_fromClient, result);
        }
    }
}