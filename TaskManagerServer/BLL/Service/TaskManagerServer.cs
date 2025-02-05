using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class TaskManagerServer
    {
        private readonly TcpListener listener;
        
        private ConcurrentDictionary<string, int> handlers;

        public TaskManagerServer(IPAddress address, int port)
        {
           listener = new TcpListener(address, port);
        }

        public async Task StartAsync()
        {
            listener.Start();
            Console.WriteLine("SERVER STARTED");
            while(true)
            {
                try
                {
                    var client = await listener.AcceptTcpClientAsync();
                    var handler = new ClientHandler(client, this);
                    _ = handler.HandleClientAsync();
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void Stop() 
        {
            listener.Stop();
        }
    }

}
