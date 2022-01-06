using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Database;
using Model;
using Model.RequestModels;
using Model.ResponseModels;
using Newtonsoft.Json;

namespace ServerHTTP
{
    public class UserController
    {
        public static Action<string, TcpClient> RegisterUser = Register;
        private static DBConnector dBConnector = DBConnector.GetInstance();

        private static void Register(string data, TcpClient client)
        {
            RegisterRequest userRequest = JsonConvert.DeserializeObject<RegisterRequest>(data);
            User user = new() { Username = userRequest.Username, Password = userRequest.Password };
            user.Session = Guid.NewGuid();
            user.id= Guid.NewGuid();
            dBConnector.insertUser(user);
            AuthenticateResponse authenticateResponse = new() { Authorization = user.Session};
            Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(authenticateResponse)));
            response.Post(client.GetStream());
        }
    }
}
