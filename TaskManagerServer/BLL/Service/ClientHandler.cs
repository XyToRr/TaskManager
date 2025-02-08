using Domain.Models;
using Microsoft.EntityFrameworkCore;
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
                    client.Close();
                    server.handlers.Remove(clientToken, out var user);
                    break;
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
                        await CreateProjectAsync(message);
                    break;
                case MessageType.ProjectListRequest:
                    if (IsTokenCorrect(message.Token))
                        await SendProjectListUpdate(message.Token);
                    break;
                case MessageType.TaskCreationRequest:
                    break;
                case MessageType.FindUser:
                    if (IsTokenCorrect(message.Token))
                       await SendUsersWithMatchingLogins(message.Content);
                    break;
                case MessageType.TaskAssignment:
                    break;
                case MessageType.TaskCompletion:
                    break;
                case MessageType.TaskStateUpdate:
                    break;
                case MessageType.AddUserToProject:
                    if (IsTokenCorrect(message.Token))
                        break;
                    break;
                default:
                    break;
            }
        }

        private async Task SendUsersWithMatchingLogins(string login)
        {
            var users = userService.FindByMatchingLogin(login);
            await SendMessage(new Message
            {
                MessageType = MessageType.FindUser,
                Content = JsonSerializer.Serialize(users),
                Token = clientToken
            });
        }

        private async Task HandleLoginRequest(string requestData)
        {
            var userInfo = JsonSerializer.Deserialize<User>(requestData);
            if (userInfo == null)
            {
                await SendMessage(new Message { MessageType = MessageType.LoginDecline });
                return;
            }


            var user = userService.FindUserByLoginAndPassword(userInfo);
            if (user == null)
            {
                await SendMessage(new Message { MessageType = MessageType.LoginDecline });
                return;
            }

            clientToken = GenerateToken();
            server.handlers.TryAdd(clientToken, user.Id);
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
            if (userService.FindUserByLoginAndPassword(userInfo) != null)
            {
                await SendMessage(new Message { MessageType = MessageType.RegisterDecline });
            }

            await userService.AddAsync(userInfo);
            await SendMessage(new Message { MessageType = MessageType.RegisterAccept });
        }

     

        public async Task SendMessage(Message message)
        {
            var messageJson = JsonSerializer.Serialize(message);
            await writer.WriteLineAsync(messageJson);
        }

        private async Task CreateProjectAsync(Message message)
        {
            var user = await userService.GetByCondition(u => u.Id == server.handlers[message.Token]).FirstAsync();
            if (user == null)
            {
                await SendProjectListUpdate();
                return;
            }

            var project = JsonSerializer.Deserialize<Project>(message.Content);
            if (project == null)
            {
                await SendProjectListUpdate();
                return;
            }

            try
            {
                await projectService.AddUser(user.Id, Role.Owner, project.Id);
                await SendProjectListUpdate(message.Token);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
                
        }

        private async Task SendProjectListUpdate(string token = null)
        {
            if (string.IsNullOrEmpty(token))
                await SendMessage(new Message
                {
                    Content = string.Empty,
                    MessageType = MessageType.ProjectListUpdate
                });

            var user = await userService.GetByCondition(u => u.Id == server.handlers[token]).FirstAsync();
            var projects = projectService.GetByCondition(p => p.Users.Select(u => u.UserId).Contains(user.Id)).ToList();


            await SendMessage(new Message
            {
                Content = JsonSerializer.Serialize(projects),
                MessageType = MessageType.ProjectListUpdate
            });
        }

        private bool IsTokenCorrect(string token) 
        {
            return this.clientToken == token;
        }

        private async Task AddUserToProject(string dataJson)
        {
            var userToProject = JsonSerializer.Deserialize<UserToProject>(dataJson);
            if (userToProject == null)
            {
                await SendMessage(new Message()
                {
                    Content = JsonSerializer.Serialize(false),
                    MessageType = MessageType.AddUserToProject,
                    Token = clientToken
                });
                return;
            }

            if (userService.GetByCondition(u => u.Id == userToProject.UserId).First() == null)
            {
                await SendMessage(new Message()
                {
                    Content = JsonSerializer.Serialize(false),
                    MessageType = MessageType.AddUserToProject,
                    Token = clientToken
                });
                return;
            }

            if (projectService.GetByCondition(p => p.Id == userToProject.RepositoryId).First() == null)
            {
                await SendMessage(new Message()
                {
                    Content = JsonSerializer.Serialize(false),
                    MessageType = MessageType.AddUserToProject,
                    Token = clientToken
                });
                return;
            }
               

            await projectService.AddUser(userToProject.UserId, Role.Worker, userToProject.RepositoryId);
            await SendMessage(new Message()
            {
                Content = JsonSerializer.Serialize(true),
                MessageType = MessageType.AddUserToProject,
                Token = clientToken
            });
        }
    }
}
