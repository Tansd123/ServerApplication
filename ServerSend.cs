using System;

namespace ServerApplication
{
    public class ServerSend
    {
        public static void SendTCPData(int _toClient, int msgid, Packet _packet)
        {
            _packet.WriteLength();
            Network.Clients[_toClient].SendData(_packet);
            Console.WriteLine("[ID: " + _toClient + "]Send Msg: " + msgid);
        }
        
        private static void SendTCPDataToAll(int _exceptClient,Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= 100; i++)
            {
                if (i != _exceptClient)
                {
                    Network.Clients[i].SendData(_packet);
                }
            }
        }
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);
                SendTCPData(_toClient,(int)ServerPackets.welcome , _packet);
            }
        }

        public static void Register(int _toClient, int result)
        {
            using (Packet _packet = new Packet((int)ServerPackets.register))
            {
                _packet.Write(result);
                SendTCPData(_toClient, (int)ServerPackets.register, _packet);
               
            }
        }

        public static void Login(int _toClient, int result, int Uin)
        {
            using (Packet _packet = new Packet((int)ServerPackets.login))
            {
                _packet.Write(result);
                _packet.Write(Uin);
                SendTCPData(_toClient, (int)ServerPackets.login, _packet);
               
            }
        }
        
        public static void Getacc(int _toClient, int result)
        {
            using (Packet _packet = new Packet((int)ServerPackets.getacc))
            {
                _packet.Write(result);
                SendTCPData(_toClient, (int)ServerPackets.getacc, _packet);
               
            }
        }

        public static void Regacc(int _toClient, int result)
        {
            using (Packet _packet = new Packet((int)ServerPackets.createacc))
            {
                _packet.Write(result);
                SendTCPData(_toClient, (int)ServerPackets.createacc, _packet);
               
            }
        }
    }
}