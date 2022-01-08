using Database;
using Model;
using Model.ResponseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerHTTP
{
    public class StatsController
    {
        public static Action<string, TcpClient, string> GetUser = Get;
        private static DBConnector dBConnector = DBConnector.GetInstance();

        private static void Get(string data, TcpClient client, string auth)
        {
            if (auth != "")
            {
                User userSession = dBConnector.getUserBySession(auth);
                if (userSession is not null)
                {
                    Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new EloResponse() { elo= userSession.ELO})));
                    response.Post(client.GetStream());
                }
                else
                {
                    ApiErrorResponse apiErrorResponse = new() { Message = "User Not Found!" };
                    Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                    response.Post(client.GetStream());
                }
            }
            else
            {
                ApiErrorResponse apiErrorResponse = new() { Message = "Not logged in!" };
                Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                response.Post(client.GetStream());
            }
        }
    }
}
