using System;
using Database;
using Model;
using ServerHTTP;

namespace Main
{
    class Program
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
                    case "connect":
                        dBConnector.CreateDB();
                        dBConnector.CreateTable();
                        break;

                    case "4":
                    case "disconnect":
                        //User user = new User() { Username = "Tamara", Password = "1111" };
                        //dBConnector.insertUser(user);
                        //dBConnector.getAllUsers();
                        ////dBConnector.getUser("Nuri");
                        //user.Username = "Aramat";
                        //dBConnector.UpdateUser(user);
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
