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
    public class DeckController
    {
        public static Action<string, TcpClient, string> ConfigureDeck = Configure;
        public static Action<string, TcpClient, string> GetDeck = Get;
        private static DBConnector dBConnector = DBConnector.GetInstance();

        private static void Configure(string data, TcpClient client, string auth)
        {
            if (auth != "")
            {
                User user = dBConnector.getUserBySession(auth);
                if (user is not null)
                {
                    Guid[] temp = JsonConvert.DeserializeObject<Guid[]>(data);
                    List<ICard> ownedCards = dBConnector.GetCardsfromUser(user.id);
                    if (temp is not null && temp.Length == 4)
                    {
                        List<ICard> cards = new();
                        bool tempbool = false; 
                        foreach (var item in temp)
                        {
                            ICard tempCard = dBConnector.GetCardsfromID(item);
                            if (tempCard is not null && ownedCards.Any(x => x.id == tempCard.id))
                                cards.Add(dBConnector.GetCardsfromID(item));
                            else
                                tempbool = true; 
                        }
                        if (!tempbool){
                            user.Deck = cards;
                            if (dBConnector.UpdateUser(user))
                            {
                                DeckResponse authenticateResponse = new() { cardIds = cards.Select(x => x.id).ToList() };
                                Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(authenticateResponse)));
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
                            ApiErrorResponse apiErrorResponse = new() { Message = "You do not own All of these Cards!" };
                            Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                            response.Post(client.GetStream());
                        }
                    }
                    else
                    {
                        ApiErrorResponse apiErrorResponse = new() { Message = "Not Enough Cards in the Deck!" };
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

        private static void Get(string data, TcpClient client, string auth)
        {
            if (auth != "")
            {
                User user = dBConnector.getUserBySession(auth);
                if (user.id!=Guid.Empty)
                {
                    DeckResponse deckResponse = new() { cardIds =  user.Deck.Select(x=>x.id).ToList()};
                    Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(deckResponse)));
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
