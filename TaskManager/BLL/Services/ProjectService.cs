using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models;
using TaskManager.Configuration.UserAuthentificationHelper;

namespace BLL.Services
{
    public class ProjectService
    {
        //private readonly TaskManagerClient _taskManagerClient;
        public Action<List<Project>> OnCreate;
        public Action<bool> OnUserAdd;

        public ProjectService()
        {
            //_taskManagerClient = ConnectionService.Instance.Client;
            ConnectionService.Instance.Client.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(Message message)
        {
            if (message.MessageType == MessageType.ProjectListUpdate)
            {
                var json = JsonSerializer.Deserialize<List<Project>>(message.Content);
                OnCreate?.Invoke(json);
            }
            if (message.MessageType == MessageType.AddUserToProject)
            {
                var json = JsonSerializer.Deserialize<bool>(message.Content);
                OnUserAdd?.Invoke(json);
            }
        }

        public async void ProjectCreate(Project project)
        {
            var json = JsonSerializer.Serialize(project);
            var Message = new Message()
            {
                MessageType = MessageType.ProjectCreationRequest,
                Content = json,
                Token = UserAuthentificationHelper.Token
            };
            await ConnectionService.Instance.Client.SendMessageAsync(Message);
        }

        public async void AddUserToProject()
        {

        }
    }
}
