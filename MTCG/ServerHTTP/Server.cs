using System;

namespace ServerHTTP
{
    public class Server
    {
        private static Server _server;
        private bool _running = false; 

        private Server()
        {

        }

        public static Server GetServer()
        {
            if (_server is not null)
                _server = new Server();
            return _server;
        }

        void start()
        {
            _running = true;
            running();
        }

        void end()
        {
            _running = false;
        }

        async void running()
        {
            
        }
    }
}
