using System;

namespace ServerApplication
{
    public class ServerSend
    {
        public static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Network.Clients[_toClient].SendData(_packet);
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
                SendTCPData(_toClient, _packet);
                Console.WriteLine("[ID: " + _toClient + "]Send Welcome");
            }
        }
    }
}