using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.RequestModels;
using Model.ResponseModels;
using Newtonsoft.Json;

namespace ServerHTTP
{
    public class UserController
    {
        public static List<User> users = new();
        public static Action<string, TcpClient> RegisterUser = Register; 

        private static void Register(string data, TcpClient client)
        {
            RegisterRequest userRequest = JsonConvert.DeserializeObject<RegisterRequest>(data);
            User user = new() { Username = userRequest.Username, Password = userRequest.Password };
            user.Session = new Guid();
            Console.WriteLine("Test");
            users.Add(user);
            AuthenticateResponse authenticateResponse = new() { Authorization = user.Session};
            Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(authenticateResponse)));
            response.Post(client.GetStream());
        }
    }
}
