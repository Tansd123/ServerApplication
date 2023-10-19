using System;
using System.Threading;

namespace ServerApplication
{
    class Program
    {
        private static Thread threadConsole;
        private static bool consoleRunning;

        static void Main(string[] args)
        {
            threadConsole = new Thread(new ThreadStart(ConsoleThread));
            threadConsole.Start();
            Network.instance.ServerStart();
            
        }

        private static void ConsoleThread()
        {
            string line;
            consoleRunning = true;

            while (consoleRunning)
            {
                line = Console.ReadLine();

                if (String.IsNullOrEmpty(line))
                {
                    consoleRunning = false;
                    return;
                }
                else
                {
                    
                }
            }
        }
    }
}