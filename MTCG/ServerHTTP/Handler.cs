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
            //TODO: try 
            string[] tokens = request.URL.Split('/');
            if (tokens.Length == 2)
            {
                if (map.TryGetValue(new Key()
                {
                    Dimension1 = tokens[1],
                    Dimension2 = request.Type
                }, out var output))
                    output(request.Data, ns);
            }
            else
                if (tokens[1] == "users" || tokens[1] == "tradings")
                    map[new Key() { Dimension1 = tokens[1], Dimension2 = request.Type }](request.Data, ns, tokens[2]);
                else
                    map[new Key() { Dimension1 = tokens[1] + "/" + tokens[2], Dimension2 = request.Type }](request.Data, ns);
        }

        public static void fillHandler()
        {
            map.Add(new Key() { Dimension1 = "users", Dimension2 = "POST" },UserController.RegisterUser);
            map.Add(new Key() { Dimension1 = "sessions", Dimension2 = "POST" },SessionsController.Session);
            //TODO REST
        }
    }
}
