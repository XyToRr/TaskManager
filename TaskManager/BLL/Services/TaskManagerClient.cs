using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models;

namespace BLL.Services
{
    public class TaskManagerClient
    {
        private readonly TcpClient _client;

        public event Action<Message> MessageReceived;
        private StreamWriter _streamWriter;
        private StreamReader _streamReader;
        private readonly string _userName;

        public TaskManagerClient(string userName)
        {
            userName = _userName;
            _client = new TcpClient();

        }

        public async Task ConnectAsync(string ipAddress, int port)
        {
            await _client.ConnectAsync(ipAddress, port);
            var stream = _client.GetStream();
            _streamReader = new StreamReader(stream);
            _streamWriter = new StreamWriter(stream) { AutoFlush = true };

            _ = ReceiveMessageAsync();
        }

        public async Task ReceiveMessageAsync()
        {
            while (true)
            {
                var messageJson = await _streamReader.ReadLineAsync();
                if (messageJson == null) break;

                var message = JsonSerializer.Deserialize<Message>(messageJson);
                MessageReceived?.Invoke(message);
            }
        }

        public async Task SendMessageAsync(Message message)
        {
            var json = JsonSerializer.Serialize(message);
            await _streamWriter.WriteLineAsync(json);
        }
    }
}

