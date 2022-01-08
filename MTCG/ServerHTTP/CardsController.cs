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
    public class CardsController
    {
        public static Action<string, TcpClient, string> GetAllCards = GetCards;
        private static DBConnector dBConnector = DBConnector.GetInstance();

        private static void GetCards(string data, TcpClient client, string auth)
        {
            if (auth != "")
            {
                User user = dBConnector.getUserBySession(auth);
                if (user is not null)
                {
                    List<ICard> cards = dBConnector.GetCardsfromUser(user.id);
                    if (cards is not null)
                    {   
                        CardsResponse cardsResponse = new() { cards = cards};
                        Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(cardsResponse)));
                        response.Post(client.GetStream());
                     
                    }
                    else
                    {
                        ApiErrorResponse apiErrorResponse = new() { Message = "No Cards Found for this User!" };
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
