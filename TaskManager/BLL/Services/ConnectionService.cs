using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ConnectionService
    {
        private static ConnectionService instance;

        public TaskManagerClient Client { get; private set; }
        public static ConnectionService Instance
        {
            get
            {
                if(instance == null)
                    instance = new ConnectionService();
                return instance;
            }
        }
           
        private ConnectionService()
        {
            Client = new TaskManagerClient();
            Task.Run(() =>Client.ConnectAsync("127.0.0.1", 5000));
        }
    }
}
