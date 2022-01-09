using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Model;
using Model.RequestModels;
using Model.ResponseModels;
using Newtonsoft.Json;

namespace ServerHTTP
{
    public class UserController
    {
        public static Action<string, TcpClient, string> RegisterUser = Register;
        public static Action<string, TcpClient, string, string> GetUser = Get;
        public static Action<string, TcpClient, string, string> SetUser = Set;
        private static DBConnector dBConnector = DBConnector.GetInstance();

        private static void Register(string data, TcpClient client, string auth)
        {
            RegisterRequest userRequest = JsonConvert.DeserializeObject<RegisterRequest>(data);
            User user = new() { Username = userRequest.Username, Password = userRequest.Password };
            user.Session = Guid.NewGuid();
            user.id= Guid.NewGuid();
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: user.Password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            user.Password = hashed; 
            user.saltkey= BitConverter.ToString(salt);
            dBConnector.insertUser(user);
            AuthenticateResponse authenticateResponse = new() { Authorization = user.Session};
            Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(authenticateResponse)));
            response.Post(client.GetStream());
            //TODO: DUmmy check
        }

        private static void Get(string data, TcpClient client, string auth, string userid)
        {
            if (auth != "")
            {
                User userSession = dBConnector.getUserBySession(auth);
                if (userSession is not null)
                {
                    User userId = dBConnector.getUserByID(userid);
                    if (userId is not null)
                    {
                        Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(userSession)));
                        response.Post(client.GetStream());

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

        private static void Set(string data, TcpClient client, string auth, string userid)
        {
            if (auth != "")
            {
                User userSession = dBConnector.getUserBySession(auth);
                if (userSession is not null)
                {
                    User userId = dBConnector.getUserByID(userid);
                    if (userId is not null)
                    {

                        RegisterRequest userRequest = JsonConvert.DeserializeObject<RegisterRequest>(data);
                        userSession.Username = userRequest.Username;
                        userSession.Password = userRequest.Password; //TODO: Salt (Wie im Register und Login) 
                        dBConnector.UpdateUser(userSession);
                        Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(userSession)));
                        response.Post(client.GetStream());

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
    }
}
