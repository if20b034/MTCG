﻿using Database;
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
    public class SessionsController
    {
        public static Action<string, TcpClient> Session = GetSession;
        private static DBConnector dBConnector = DBConnector.GetInstance();

        private static void GetSession(string data, TcpClient client)
        {
            RegisterRequest user = JsonConvert.DeserializeObject<RegisterRequest>(data);
            //TODO Salt
            User findUser = dBConnector.getAllUsers().Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();
            findUser.Session = Guid.NewGuid();
            if(dBConnector.UpdateUser(findUser))
                if (findUser is not null)
                {
                    AuthenticateResponse authenticateResponse = new() { Authorization = findUser.Session };
                    Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(authenticateResponse)));
                    response.Post(client.GetStream());
                }
                else
                {
                    ApiErrorResponse authenticateResponse = new() { Message = "User Not Found!"};
                    Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(authenticateResponse)));
                    response.Post(client.GetStream());
                }
            else
            {
                ApiErrorResponse authenticateResponse = new() { Message = "Database Error! Contact Admin." };
                Response response = Response.From("200 OK", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(authenticateResponse)));
                response.Post(client.GetStream());
            }
        }
    }
}
