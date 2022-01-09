using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerHTTP
{
    public class Handler
    {
        private static Dictionary<Key, dynamic> map = new();
        public static void chooseController(Request request, TcpClient ns)
        {
            string[] tokens = request.URL.Split('/');
            if (tokens.Length == 2)
            {
                if (map.TryGetValue(new Key()
                {
                    Dimension1 = tokens[1],
                    Dimension2 = request.Type
                }, out var output))
                    output(request.Data, ns, request.Authorization);
            }
            else
                if (tokens[1] == "transactions"|| tokens[1] == "battles")
                    if (map.TryGetValue(new Key() { Dimension1 = tokens[1] + "/" + tokens[2], Dimension2 = request.Type }, out var output))
                        output(request.Data, ns, request.Authorization);
                    else
                    { }
                else
                    if (map.TryGetValue(new Key() { Dimension1 = tokens[1]+"/", Dimension2 = request.Type }, out var output))
                        output(request.Data, ns, request.Authorization, tokens[2]);
        }

        public static void fillHandler()
        {
            map.Add(new Key() { Dimension1 = "users", Dimension2 = "POST" },UserController.RegisterUser);
            map.Add(new Key() { Dimension1 = "sessions", Dimension2 = "POST" },SessionsController.Session);
            map.Add(new Key() { Dimension1 = "packages", Dimension2 = "POST" }, PackagesController.CreatePackage);
            map.Add(new Key() { Dimension1 = "transactions/packages", Dimension2 = "POST" }, TransactionsController.BuyPackage);
            map.Add(new Key() { Dimension1 = "cards", Dimension2 = "GET" }, CardsController.GetAllCards);
            map.Add(new Key() { Dimension1 = "deck", Dimension2 = "GET" }, DeckController.GetDeck);
            map.Add(new Key() { Dimension1 = "deck", Dimension2 = "PUT" }, DeckController.ConfigureDeck);
            map.Add(new Key() { Dimension1 = "users/", Dimension2 = "GET" }, UserController.GetUser);
            map.Add(new Key() { Dimension1 = "users/", Dimension2 = "PUT" }, UserController.SetUser);
            map.Add(new Key() { Dimension1 = "stats", Dimension2 = "GET" }, StatsController.GetUser);
            map.Add(new Key() { Dimension1 = "score", Dimension2 = "GET" }, ScoreController.GetScore);

            map.Add(new Key() { Dimension1 = "tradings", Dimension2 = "GET" }, ScoreController.GetScore);
            map.Add(new Key() { Dimension1 = "tradings/", Dimension2 = "DELETE" }, ScoreController.GetScore);
            map.Add(new Key() { Dimension1 = "tradings", Dimension2 = "POST" }, ScoreController.GetScore);
            map.Add(new Key() { Dimension1 = "tradings/", Dimension2 = "POST" }, ScoreController.GetScore);

            map.Add(new Key() { Dimension1 = "battles", Dimension2 = "POST" }, BatlleController.BattleStart);
            map.Add(new Key() { Dimension1 = "battles/Add", Dimension2 = "POST" }, BatlleController.AddBattle);
        }
    }
}
