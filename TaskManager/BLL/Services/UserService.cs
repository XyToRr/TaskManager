using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models;
using TaskManager.Configuration.UserAuthentificationHelper;

namespace BLL.Services
{
    public class UserService
    {
        public Action<bool> RegisterRequestReceived;
        public Action<bool> LoginRequestReceived;

        private TaskManagerClient _taskManagerClient;

        public UserService(TaskManagerClient client) 
        {
            _taskManagerClient = client;
            _taskManagerClient.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(Message message)
        {
            if (message.MessageType == MessageType.RegisterAccept)
            {
                RegisterRequestReceived?.Invoke(true);
            }
            if (message.MessageType == MessageType.RegisterDecline) 
            { 
                RegisterRequestReceived?.Invoke(false); 
            }
            if (message.MessageType == MessageType.LoginAccept)
            {
                UserAuthentificationHelper.Token = message.Token;
                LoginRequestReceived?.Invoke(true);
            }
            if (message.MessageType == MessageType.LoginDecline)
            {
                LoginRequestReceived?.Invoke(false);
            }

        }

        public async Task RegisterRequest(User user)
        {
            var json = JsonSerializer.Serialize(user);
            var Message = new Message()
            {
                MessageType = MessageType.RegisterRequest,
                Content = json
            };
            await _taskManagerClient.SendMessageAsync(Message);
            
        }

        public async Task LoginRequest(User user)
        {
            var json = JsonSerializer.Serialize(user);
            var Message = new Message()
            {
                MessageType = MessageType.LoginRequest,
                Content = json
            };
            await _taskManagerClient.SendMessageAsync(Message);
        }
    }
}
