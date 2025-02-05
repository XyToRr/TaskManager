using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace BLL.Services
{
    public class UserService
    {
        private readonly TaskManagerClient _taskManagerClient;
        public async Task RegisterRequest()
        {
            var Message = new Message()
            {

            };
            await _taskManagerClient.SendMessageAsync(Message);
            var 
        }
    }
}
