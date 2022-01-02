using Model;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerHTTP
{
    public class Server
    {
        private static Server _server;
        private static bool _running = false;
        public bool running { get { return _running; } }
        private static TcpListener listener;
        private Thread serverThread;

        private Server()
        {

        }

        public static Server GetServer()
        {
            if (_server is null)
                _server = new Server();
            return _server;
        }

        public void start()
        {
            _running = true;
            serverThread = new(new ThreadStart(Run));
            serverThread.Start();
        }

        public void end()
        {
            try
            {
                _running = false;
                listener.Stop();
                serverThread.Abort();
            }catch(Exception e)
            {
                Console.WriteLine("error occurred: " + e.Message);
            }
        }

        static void Run()
        {
            listener = new TcpListener(IPAddress.Loopback, 10001);
            listener.Start(5);

            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);
            while (_running)
            {
                try
                {
                    var socket = listener.AcceptTcpClient(); //TODO: Create Task here
                    NetworkStream stream = socket.GetStream();
                    string data=null;
                    Byte[] bytes = new Byte[1024];
                    int i;
                    i = stream.Read(bytes, 0, bytes.Length);
                    
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    
                    Console.WriteLine("Received: {0}", data);
                    Request request = Request.GetRequest(data);

                    Thread clientThread = new(()=>Handler.chooseController(request, socket));
                    clientThread.Start();

                    // Controller Responses himself 
                    
                }
                catch (Exception exc)
                {
                    Console.WriteLine("error occurred: " + exc.Message);
                }
            }
        }

        private static void fillHandler()
        {

        }
    }
}
