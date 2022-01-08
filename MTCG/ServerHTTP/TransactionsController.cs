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
    public class TransactionsController
    {
        public static Action<string, TcpClient, string> BuyPackage = Buy;
        private static DBConnector dBConnector = DBConnector.GetInstance();

        private static void Buy(string data, TcpClient client, string auth)
        {
            if (auth != "")
            {
                User user = dBConnector.getUserBySession(auth);
                if (user is not null)
                {
                    Package package = dBConnector.GetAllPackages().First();
                    if (user.buyPack(package))
                    {
                        if (dBConnector.DeletePackage(package.id))
                        {
                            bool temp = false; 
                            foreach (var item in package.Cards)
                            {
                                if (!dBConnector.UpdateCard(item, user))
                                    temp = true; 
                            }
                            if (!temp)
                            {
                                PackageResponse authenticateResponse = new() { cards = package.Cards };
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
                            ApiErrorResponse apiErrorResponse = new() { Message = "Database Error. Contact Admin!" };
                            Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(apiErrorResponse)));
                            response.Post(client.GetStream());
                        }
                    }
                    else
                    {
                        ApiErrorResponse apiErrorResponse = new() { Message = "Not Enough Coins!" };
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
