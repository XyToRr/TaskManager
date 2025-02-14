using Domain.Models;
using Domain.Models.ClientModels;
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
                    if (IsTokenCorrect(message.Token))
                        await AddTaskToProjectAsync(message);
                    break;
                case MessageType.FindUser:
                    if (IsTokenCorrect(message.Token))
                       await SendUsersWithMatchingLogins(message.Content);
                    break;
                case MessageType.ProjectTasksListRequest:
                    if (IsTokenCorrect(message.Token))
                        await SendProjectTaskToUser(message);
                    break;
                case MessageType.TaskAssignment:
                    break;
                case MessageType.TaskCompletion:
                    break;
                case MessageType.TaskStateUpdate:
                    break;
                case MessageType.AddUserToProject:
                    if (IsTokenCorrect(message.Token))
                        await AddUserToProject(message.Content);
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
            if (server != null)
            {
                server.handlers.TryAdd(clientToken, user.Id);
            }
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

        private async Task SendProjectTaskToUser(Message message)
        {
            var user = await userService.GetByCondition(u => u.Id == server.handlers[message.Token]).FirstAsync();
            if (user == null)
            {
                await SendMessage(new Message
                {
                    Content = string.Empty,
                    MessageType = MessageType.ProjectTasksListRequest
                });
                return;
            }

            var project = await projectService.GetByCondition(p => p.Id == int.Parse(message.Content)).Include(p => p.Tasks).FirstAsync();

            if (project == null)
            {
                await SendMessage(new Message
                {
                    Content = string.Empty,
                    MessageType = MessageType.ProjectTasksListRequest
                });
                return;
            }

            var tasks = project.Tasks.Where(t => t.AssignedUserId == user.Id).ToList();
            
            var clientTasks = new List<ClientTaskInfo>();
            foreach (var task in tasks)
            {
                clientTasks.Add(GetTaskClientrInfo(task));
            }

            var role = project.Users.Where(up => up.UserId == user.Id).Select(up => up.Role).First();

            await SendMessage(new Message
            {
                Content = JsonSerializer.Serialize(new TaskListRequestResponce()
                {
                    Tasks = clientTasks,
                    Role = role,
                }),
                Token = clientToken,
                MessageType = MessageType.ProjectTasksListRequest
            });
        }

        private ClientTaskInfo GetTaskClientrInfo(TaskModel task)
        {
            return new ClientTaskInfo()
            {
                Name = task.Name,
                Description = task.Description,
                CreatedAt = task.CreatedAt.ToShortDateString(),
                Deadline = task.DeadLine.ToShortDateString(),
                Priority = task.Priority.ToString(),
                Status = task.Status.ToString(),
                CreatedBy = task.CreatedUser.Login,
            };
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
                project.CreatedAt = DateTime.Now;
                await projectService.AddAsync(project);
                await projectService.AddUser(user.Id, Role.Owner, project.Id);
                await SendProjectListUpdate(message.Token);
                await projectService.SaveAsync();

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
            var projects = await projectService.GetByCondition(p => p.Users.Select(u => u.UserId).Contains(user.Id)).Include(p => p.Users).ToListAsync();

            var clientProjectsInfo = new List<ProjectOnClient>();
            foreach(var proj in projects)
            {
                clientProjectsInfo.Add(GetClientProjectInfo(proj, user));
            }

            //var projectsJson =;
            await SendMessage(new Message
            {
                Content = JsonSerializer.Serialize(clientProjectsInfo),
                MessageType = MessageType.ProjectListUpdate
            });
        }


        private ProjectOnClient GetClientProjectInfo(Project proj, User user)
        {
            return new ProjectOnClient()
            {
                Id = proj.Id,
                Name = proj.Name,
                Description = proj.Description,
                CreatedDate = proj.CreatedAt.ToShortDateString(),
                TaskCount = proj.Tasks.Count,
                Role = proj.Users.Where(up => up.UserId == user.Id).Select(up => up.Role).First().ToString(),

            };

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
               

            await projectService.AddUser(userToProject.UserId, userToProject.Role, userToProject.RepositoryId);
            await SendMessage(new Message()
            {
                Content = JsonSerializer.Serialize(true),
                MessageType = MessageType.AddUserToProject,
                Token = clientToken
            });
        }

        private async Task AddTaskToProjectAsync(Message message)
        {
            var newTaskData = JsonSerializer.Deserialize<TaskModel>(message.Content);

            var userCreated = await userService.GetByCondition(u => u.Id == server.handlers[message.Token]).FirstAsync();

            if (userCreated == null)
                await SendMessage(new Message()
                {
                    Content = "",
                    MessageType = MessageType.TaskCreationRequest,
                    Token = clientToken
                });

            var project = projectService.GetByCondition(p => p.Id == newTaskData.RepositoryId).First();
            project.Tasks.Add(newTaskData);
            await SendMessage(new Message()
            {
                Content = JsonSerializer.Serialize(project),
                MessageType = MessageType.TaskCreationRequest,
                Token = clientToken
            });
        }
    }
}
