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
    public class ScoreController
    {
        public static Action<string, TcpClient, string> GetScore = Get;
        private static DBConnector dBConnector = DBConnector.GetInstance();

        private static void Get(string data, TcpClient client, string auth)
        {
            if (auth != "")
            {
                User userSession = dBConnector.getUserBySession(auth);
                if (userSession is not null)
                {
                    List<User> users = dBConnector.getAllUsers();
                    if(users is not null)
                    {
                        List<ScoreResponse> scoreResponses = new();
                        users.OrderBy(x => x.ELO).Select((Value, Index) => new { Value, Index });
                        foreach (var item in users.OrderBy(x => x.ELO).Select((value, index) => new { value, index }))
                        {
                            scoreResponses.Add(new ScoreResponse() { elo = item.value.ELO, Username = item.value.Username, rank = item.index });
                        }
                        Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(scoreResponses)));
                        response.Post(client.GetStream());
                    }
                    else
                    {
                        ApiErrorResponse apiErrorResponse = new() { Message = "Database Error. Contact Admin!" };
                        Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                        response.Post(client.GetStream());
                    }
                    
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
