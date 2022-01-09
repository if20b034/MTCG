using Database;
using Model;
using Model.RequestModels;
using Model.ResponseModels;
using MTCG;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerHTTP
{
    public class BatlleController
    {
        public static Action<string, TcpClient, string> BattleStart = Battling;
        public static Action<string, TcpClient, string> AddBattle = Add;
        private static DBConnector dBConnector = DBConnector.GetInstance();
        private static Battle battle = Battle.GetInstance();

        private static void Battling(string data, TcpClient client, string auth)
        {
            if (auth != "") { 
                User user = dBConnector.getUserBySession(auth);
                if (user is not null)
                {
                    if (user.Deck.Count == 4) //NO Secureity for same player joining twice
                    {
                        if (Battle.loadingUser is not null) //Spieler 2
                        {
                            User user1 = Battle.loadingUser;
                            Battle.loadingUser = user;

                            BattleResult result = battle.Fight(user, user1);
                            if (result.winner)
                            {
                                user.ELO = user.ELO + 5;
                            }
                            else
                            {
                                user.ELO = user.ELO - 5;
                            }
                            if (dBConnector.UpdateUser(user))
                            {
                                Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(result)));
                                response.Post(client.GetStream());
                            }
                            else
                            {
                                ApiErrorResponse apiErrorResponse = new() { Message = "Database Error. Contact Admin!" };
                                Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                                response.Post(client.GetStream());
                            }
                        }
                        else //Spieler 1
                        {
                            Battle.loadingUser = user;
                            while (Battle.loadingUser.id == user.id)
                            {
                                // Wait
                            }
                            User user2 = Battle.loadingUser;
                            Battle.loadingUser = null;
                            if (user2 is not null)
                            {
                                BattleResult result = battle.Fight(user, user2);
                                Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(result)));
                                response.Post(client.GetStream());
                            }
                            else
                            {
                                ApiErrorResponse apiErrorResponse = new() { Message = "User Error. Contact Admin!" };
                                Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                                response.Post(client.GetStream());
                            }
                        }
                    }
                    else
                    {
                        ApiErrorResponse apiErrorResponse = new() { Message = "Not a real Deck in use!" };
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

        private static void Add(string data, TcpClient client, string auth)
        {
            if (auth != "")
            {
                User user = dBConnector.getUserBySession(auth); // Admin 
                if (user is not null&& user.id==DBConnector.Admin.id)
                {
                    BattleAddRequest battleAddRequest = JsonConvert.DeserializeObject<BattleAddRequest>(data);
                    if(battleAddRequest is not null)
                    {
                        Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(battle.addType(battleAddRequest.key, battleAddRequest.multiplier))));
                        response.Post(client.GetStream());
                    }
                    else
                    {
                        ApiErrorResponse apiErrorResponse = new() { Message = "Key not Buildable!" };
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
