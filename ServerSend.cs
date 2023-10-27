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
        
        public static void Getacc(int _toClient, int result, int Uin, string Nickname, int Level, float Exp, int Gold, float MaxExp)
        {
            using (Packet _packet = new Packet((int)ServerPackets.getacc))
            {
                _packet.Write(result);
                _packet.Write(Uin);
                _packet.Write(Nickname);
                _packet.Write(Level);
                _packet.Write(Exp);
                _packet.Write(Gold);
                _packet.Write(MaxExp);
                Console.WriteLine($"{Exp}");
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

        public static void GainExp(int _toClient, float Exp)
        {
            using (Packet _packet = new Packet((int)ServerPackets.gainexp))
            {
                _packet.Write(Exp);
                SendTCPData(_toClient, (int)ServerPackets.gainexp, _packet);
               
            }
        }

        public static void LevelUp(int _toClient, float Exp, float MaxExp, int Level)
        {
            using (Packet _packet = new Packet((int)ServerPackets.levelup))
            {
                _packet.Write(Exp);
                _packet.Write(MaxExp);
                _packet.Write(Level);
                SendTCPData(_toClient, (int)ServerPackets.levelup, _packet);
               
            }
        }

        public static void GetShop(int _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.getshop))
            {
                _packet.Write(Network.Shops.Count);
                int fish = 0, trangtri = 0, taphoa = 0, sukien = 0, unknow =0;
                foreach (var Shop in Network.Shops)
                {
                    _packet.Write(Shop.Type);
                    if (Shop.Type == 1) fish++;
                    else if (Shop.Type == 2) trangtri++;
                    else if (Shop.Type == 3) taphoa++;
                    else if (Shop.Type == 4) sukien++;
                    else unknow++;
                    _packet.Write(Shop.ItemId);
                    _packet.Write(Shop.Name);
                    _packet.Write(Shop.Level);
                    _packet.Write(Shop.Time);
                    _packet.Write(Shop.Exp);
                    _packet.Write(Shop.Gold);
                    _packet.Write(Shop.Cost);
                }
                _packet.Write(fish);
                _packet.Write(trangtri);
                _packet.Write(taphoa);
                _packet.Write(sukien);
                _packet.Write(unknow);
                SendTCPData(_toClient, (int)ServerPackets.getshop, _packet);
               
            }
        }
        
    }
}