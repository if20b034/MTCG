using Database;
using Model;
using Model.RequestModels;
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
    public class TradingsController
    {
        public static Action<string, TcpClient, string> GetTradings = Get;
        public static Action<string, TcpClient, string > PostTradings = Post;
        public static Action<string, TcpClient, string, string> DeleteTrading = Delete;
        public static Action<string, TcpClient, string, string> AcceptTrading = Accept;
        private static DBConnector dBConnector = DBConnector.GetInstance();

        private static void Get(string data, TcpClient client, string auth)
        {
            if (auth != "")
            {
                User userSession = dBConnector.getUserBySession(auth);
                if (userSession is not null)
                {
                    List<Trade> trades = dBConnector.GetAllTrades();
                    if (trades is not null)
                    {
                        Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(trades)));
                        response.Post(client.GetStream());

                    }
                    else
                    {
                        ApiErrorResponse apiErrorResponse = new() { Message = "Database Error. Contact Admin" };
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

        private static void Post(string data, TcpClient client, string auth)
        {
            if (auth != "")
            {
                User userSession = dBConnector.getUserBySession(auth);
                if (userSession is not null)
                {
                    TradeRequest tradeRequest = JsonConvert.DeserializeObject<TradeRequest>(data);
                    if (tradeRequest is not null)
                    {
                        Trade shop = new() { CardId = tradeRequest.CardToTrade };
                        shop.TradingCard = new() {MinimumDamage= tradeRequest.MinimumDamage, CardType= tradeRequest.Type, ElementType= tradeRequest.ElementType }; 
                        shop.id = Guid.NewGuid();
                        if(dBConnector.InsertTrade(shop))
                        {
                            Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(shop)));
                            response.Post(client.GetStream());
                        }
                        else
                        {
                            ApiErrorResponse apiErrorResponse = new() { Message = "Did not get Any Data!" };
                            Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                            response.Post(client.GetStream());
                        }
                    }
                    else
                    {
                        ApiErrorResponse apiErrorResponse = new() { Message = "You are not Authorized! " };
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

        private static void Delete(string data, TcpClient client, string auth, string id)
        {
            if (auth != "")
            {
                User userSession = dBConnector.getUserBySession(auth);
                if (userSession is not null)
                {
                    Trade trade = dBConnector.GetTrade(Guid.Parse(id));
                    if (trade is not null)
                    {
                        if (dBConnector.DeleteTrade(Guid.Parse(id))){
                            Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(trade)));
                            response.Post(client.GetStream());
                        }
                        else
                        {
                            ApiErrorResponse apiErrorResponse = new() { Message = "Database Error. Contact Admin" };
                            Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                            response.Post(client.GetStream());
                        }
                    }
                    else
                    {
                        ApiErrorResponse apiErrorResponse = new() { Message = "Database Error. Contact Admin" };
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

        private static void Accept(string data, TcpClient client, string auth, string id)
        {
            if (auth != "")
            {
                User userSession = dBConnector.getUserBySession(auth);
                if (userSession is not null)
                {
                    Trade trade = dBConnector.GetTrade(Guid.Parse(id));
                    if (trade is not null)
                    {
                        if(dBConnector.GetCardsfromUser(userSession.id).All(x=>x.id!=trade.id))
                        {
                            ICard card = dBConnector.GetCardsfromID(Guid.Parse(data)); //card = card from User 
                            if (trade.TradingCard.CardType == typeof(Monster).ToString().ToLower() && card.GetType() == typeof(Monster))
                            {
                                if (trade.TradingCard.ElementType == card.ElementType && trade.TradingCard.MinimumDamage <= card.Damage)
                                {
                                    Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(trade)));
                                    response.Post(client.GetStream());
                                }
                                else
                                {
                                    ApiErrorResponse apiErrorResponse = new() { Message = "The Card does not meet the Requirements!" };
                                    Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                                    response.Post(client.GetStream());
                                }
                            }
                            else 
                            {
                                 if (trade.TradingCard.CardType == typeof(Spell).ToString().ToLower() && card.GetType() == typeof(Spell))
                                 {
                                    if (trade.TradingCard.ElementType == card.ElementType && trade.TradingCard.MinimumDamage <= card.Damage)
                                    {
                                        Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(trade)));
                                        response.Post(client.GetStream());
                                    }
                                    else
                                    {
                                        ApiErrorResponse apiErrorResponse = new() { Message = "The Card does not meet the Requirements!" };
                                        Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                                        response.Post(client.GetStream());
                                    }
                                }
                                 else
                                 {
                                    ApiErrorResponse apiErrorResponse = new() { Message = "The Card does not meet the Requirements!" };
                                    Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                                    response.Post(client.GetStream());
                                 }
                            } 
                        }
                        else
                        {
                            ApiErrorResponse apiErrorResponse = new() { Message = "Do not Trade with yourself!" };
                            Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                            response.Post(client.GetStream());
                        }

                    }
                    else
                    {
                        ApiErrorResponse apiErrorResponse = new() { Message = "Trade Not Found!" };
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
