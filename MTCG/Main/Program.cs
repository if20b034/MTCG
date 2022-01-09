using System;
using System.Diagnostics;
using Database;
using Model;
using ServerHTTP;

namespace Main
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Server server = Server.GetServer();
            DBConnector dBConnector = DBConnector.GetInstance();
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
                    case "create":
                        dBConnector.CreateDB();
                        dBConnector.CreateTables();
                        break;

                    case "9":
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
            Console.WriteLine("Datenbank erstellen (create) (3)");
            Console.WriteLine("Quit (quit) (9)");
            Console.WriteLine("----------------------------------------------");
        }

        private static void quit()
        {
            System.Environment.Exit(0);
        }
    }
}
