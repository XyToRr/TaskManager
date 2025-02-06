using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
        private ProjectService projectService;

        private string clientToken;
        private User user;
        public ClientHandler(TcpClient client, TaskManagerServer server) 
        {
            this.client = client;
            this.server = server;

            var stream = this.client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            userService = DependencyInjector.ServiceProvider.GetService<UserService>();
            projectService = DependencyInjector.ServiceProvider.GetService<ProjectService>();
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

        private async void HandleMessage(Message message)
        {
            switch (message.MessageType)
            {
                case MessageType.RegisterRequest:
                    await HandleRegisterRequest(message.Content);
                    break;
                case MessageType.LoginRequest:
                    await HandleLoginRequest(message.Content);
                    break;
                case MessageType.ProjectCreationRequest:
                    if(IsTokenCorrect(message.Token))
                        await CreateProjectAsync(message.Content);
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
                default:
                    break;
            }
        }

        private async Task HandleLoginRequest(string requestData)
        {
            var userInfo = JsonSerializer.Deserialize<User>(requestData);
            if (userInfo == null)
            {
                await SendMessage(new Message { MessageType = MessageType.LoginDecline });
                return;
            }


            user = FindUser(userInfo);
            if (user == null)
            {
                await SendMessage(new Message { MessageType = MessageType.LoginDecline });
                return;
            }

            clientToken = GenerateToken();
            await SendMessage(new Message
            {
                Token = clientToken,
                Content = JsonSerializer.Serialize(user),
                MessageType = MessageType.LoginAccept
            });
        }

        private string GenerateToken()
        {
            var myuuid = Guid.NewGuid();
            return myuuid.ToString();
        }
        private async Task HandleRegisterRequest(string requestData)
        {
            var userInfo = JsonSerializer.Deserialize<User>(requestData);
            if (userInfo == null)
            {
                await SendMessage(new Message { MessageType = MessageType.RegisterDecline });
            }
            if (FindUser(userInfo) != null)
            {
                await SendMessage(new Message { MessageType = MessageType.RegisterDecline });
            }

            await userService.AddAsync(userInfo);
            await SendMessage(new Message { MessageType = MessageType.RegisterAccept });
        }

        private User FindUser(User user)
        {
            return userService.GetByCondition(u => u.Login == user.Login && u.PasswordHash == user.PasswordHash).FirstOrDefault();
        }

        public async Task SendMessage(Message message)
        {
            var messageJson = JsonSerializer.Serialize(message);
            await writer.WriteLineAsync(messageJson);
        }

        private async Task CreateProjectAsync(string newProjectJson)
        {
            if (user == null)
            {
                await SendMessage(new Message
                {
                    Content = "",
                    MessageType = MessageType.ProjectListUpdate
                });
                return;
            }
                
            var project = JsonSerializer.Deserialize<Project>(newProjectJson);
            if (project == null)
            {
                await SendMessage(new Message
                {
                    Content = "",
                    MessageType = MessageType.ProjectListUpdate
                });
                return;
            }

            try
            {
                await projectService.AddUser(user.Id, Role.Owner, project.Id);
                await SendMessage(new Message
                {
                    Content = JsonSerializer.Serialize(projectService.GetByCondition(p => p.Users.Select(u => u.UserId).Contains(user.Id)).ToList()),
                    MessageType = MessageType.ProjectListUpdate
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
                
        }

        private bool IsTokenCorrect(string token) 
        {
            return this.clientToken == token;
        }
    }
}
