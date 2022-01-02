using System;
using ServerHTTP;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Server server = Server.GetServer();
            Handler.fillHandler();

            writeScreen();
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "1":
                    case "start":
                        if (!server.running)
                        {
                            server.start();
                            Console.WriteLine("Server gestartet! ");
                        }
                        break;

                    case "2":
                    case "close":
                        if (server.running)
                        {
                            server.end();
                            Console.WriteLine("Server gestoppt! ");
                        }
                        break;

                    case "3":
                    case "connect":
                        break;

                    case "4":
                    case "disconnect":
                        break;

                    case "5":
                    case "quit": quit(); 
                        break;

                    default: Console.WriteLine("Nicht erkannt bitte nochmal probieren ");
                        Console.ReadKey();
                        break; 
                }
                Console.Write("(Eine beliebige Taste drücken!)");
                Console.ReadKey();
                writeScreen();
            }
        }

        private static void writeScreen()
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Server Starten (start) (1)");
            Console.WriteLine("Server beenden (close) (2)");
            Console.WriteLine("Datenbank starten (connect) (3)");
            Console.WriteLine("Datenbank trennen (disconnect) (4)");
            Console.WriteLine("Quit (quit) (5)");
            Console.WriteLine("----------------------------------------------");
        }

        private static void quit()
        {

        }
    }
}
