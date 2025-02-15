using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.ClientModels;
using TaskManager.Configuration.UserAuthentificationHelper;

namespace BLL.Services
{
    public class TaskService
    {
        private readonly TaskManagerClient _taskManagerClient;
        public Action<bool> isCreated;
        public Action<TaskListRequestResponce> TasksGetRequest;

        public TaskService(TaskManagerClient taskManagerClient)
        {
            _taskManagerClient = taskManagerClient;
            _taskManagerClient.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(Message message)
        {
            if (message.MessageType == MessageType.TaskCreationRequest)
            {
                var json = JsonSerializer.Deserialize<bool>(message.Content);
                isCreated?.Invoke(json);
            }
            if (message.MessageType == MessageType.ProjectTasksListRequest)
            {
                var json = JsonSerializer.Deserialize<TaskListRequestResponce>(message.Content);
                TasksGetRequest?.Invoke(json);
            }
        }

        public async void TaskCreate(TaskModel task)
        {
            var json = JsonSerializer.Serialize(task);
            var Message = new Message()
            {
                MessageType = MessageType.TaskCreationRequest,
                Content = json,
                Token = UserAuthentificationHelper.Token
            };
            await _taskManagerClient.SendMessageAsync(Message);
        }

        public async void TaskGet(int id)
        {
            var json = JsonSerializer.Serialize(id);
            var Message = new Message()
            {
                MessageType = MessageType.ProjectTasksListRequest,
                Token = UserAuthentificationHelper.Token,
                Content = json
            };
            await _taskManagerClient.SendMessageAsync(Message);
        }
    }
}
