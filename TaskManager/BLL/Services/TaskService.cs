using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models;

namespace BLL.Services
{
    public class TaskService
    {
        private readonly TaskManagerClient _taskManagerClient;
        public Action<bool> isCreated;

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
        }

        public async void TaskCreate(TaskModel task)
        {
            var json = JsonSerializer.Serialize(task);
            var Message = new Message()
            {
                MessageType = MessageType.TaskCreationRequest,
                Content = json
            };
            await _taskManagerClient.SendMessageAsync(Message);
        }

        
    }
}
