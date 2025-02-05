using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BLL.Service
{
    public class ClientHandler
    {
        private TcpClient client;
        private TaskManagerServer server;

        private StreamReader reader;
        private StreamWriter writer;

        private UserService userService;
        public ClientHandler(TcpClient client, TaskManagerServer server) 
        {
            this.client = client;
            this.server = server;

            var stream = this.client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            userService = DependencyInjector.ServiceProvider.GetService<UserService>();
        }

        public async Task HandleClientAsync()
        {
            while (true)
            {
                try
                {
                    var messageJson = await reader.ReadLineAsync();
                    if (messageJson == null)
                        break;

                    var message = JsonSerializer.Deserialize<Message>(messageJson);
                    if(message != null)
                    {
                        HandleMessage(message);
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void HandleMessage(Message message)
        {
            switch (message.MessageType)
            {
                case MessageType.RegisterRequest:
                    HandleRegisterRequest(message.Content);
                    break;
                case MessageType.LoginRequest:
                    break;
                case MessageType.RoleChange:
                    break;
                case MessageType.ProjectCreationRequest:
                    break;
                case MessageType.TaskCreationRequest:
                    break;
                case MessageType.FindUser:
                    break;
                case MessageType.TaskAssignment:
                    break;
                case MessageType.TaskCompletion:
                    break;
                case MessageType.TaskStateUpdate:
                    break;
                case MessageType.AddUserToProject:
                    break;
            }
        }

        private async void HandleRegisterRequest(string requestData)
        {
            var userInfo = JsonSerializer.Deserialize<User>(requestData);
            if(userInfo == null)
            {
                return;
            }
            if (userService.GetByCondition(u => u.Login == userInfo.Login && u.PasswordHash == userInfo.PasswordHash).First() != null)
            {
                return;
            }

            userService.AddAsync(userInfo);
            userService.
        }
    }
}
