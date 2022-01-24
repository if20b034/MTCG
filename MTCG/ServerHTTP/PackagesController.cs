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
    public class PackagesController
    {
        public static Action<string, TcpClient, string> CreatePackage = Create;
        private static DBConnector dBConnector = DBConnector.GetInstance();

        private static void Create(string data, TcpClient client, string auth)
        {
            if (auth != "")
            {
                User user = dBConnector.getUserBySession(auth); // Admin 
                if (user is not null && user.id == DBConnector.Admin.id)
                {
                    PackageRequest packageRequest = JsonConvert.DeserializeObject<PackageRequest>(data);
                    if (packageRequest.cards is not null && packageRequest.cards.Length == 5)
                    {
                        bool temp = false;
                        ICard card;
                        List<ICard> tempCards = new();
                        foreach (var item in packageRequest.cards)
                        {
                            if (item.MonsterType != null)
                                card = new Monster() { Damage = item.Damage, ElementType = item.ElementType, MonsterType = item.MonsterType, Name = item.Name };
                            else
                                card = new Spell() { Damage = item.Damage, ElementType = item.ElementType, Name = item.Name };
                            card.id = Guid.NewGuid();
                            if (!dBConnector.InsertCard(card))
                                temp = true;
                            else
                                tempCards.Add(card);
                        }
                        if (!temp)
                        {
                            Package package = new() { id = Guid.NewGuid() };
                            if (package.addCards(tempCards))
                            {
                                if (dBConnector.InsertPackage(package))
                                {
                                    Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(package)));
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
                                ApiErrorResponse apiErrorResponse = new() { Message = "MainLogic Error. Contact Admin!" };
                                Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                                response.Post(client.GetStream());
                            }

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
                        ApiErrorResponse apiErrorResponse = new() { Message = "Not enough Cards!" };
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
