using System;
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

        public TaskManagerServer(IPAddress address, int port)
        {
           listener = new TcpListener(address, port);
        }

        public async Task StartAsync()
        {
            listener.Start();
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
