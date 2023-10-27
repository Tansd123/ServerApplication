using System;
using System.IO;
using System.Threading;



namespace ServerApplication
{
    class Program
    {
        private static Thread threadConsole;
        private static bool consoleRunning;

        static void Main(string[] args)
        {
            consoleRunning = true;
            threadConsole = new Thread(new ThreadStart(ConsoleThread));
            threadConsole.Start();
            MySql.mysql.InitDatabase();
            Network.instance.ServerStart();
        }

        

        private static void ConsoleThread()
        {
            string line;
            Console.WriteLine($"Main Thread is started. Runing at {Constants.tps} ticks per second.");
            DateTime _nextloop = DateTime.Now;
            while (consoleRunning)
            {
                GameLogic.Update();
                
            }
        }
    }
}