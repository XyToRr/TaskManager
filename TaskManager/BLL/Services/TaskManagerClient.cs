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
        private CancellationTokenSource _cancellationTokenSource;
        private bool IsConnected;

        public event Action<Message> MessageReceived;
        private StreamWriter _streamWriter;
        private StreamReader _streamReader;
        private readonly string _userName;

        public TaskManagerClient()
        {
            _client = new TcpClient();
        }

        public async Task<bool> ConnectAsync(string ipAddress, int port)
        {

            if (IsConnected) {  return false; }

            try
            {
                await _client.ConnectAsync(ipAddress, port);
            }
            catch (Exception ex) { 
                return false;
            }
            
            IsConnected = true;
            _cancellationTokenSource = new CancellationTokenSource();

            var stream = _client.GetStream();
            _streamReader = new StreamReader(stream);
            _streamWriter = new StreamWriter(stream) { AutoFlush = true };

            _ = ReceiveMessageAsync();
            return true;
        }

        public async Task ReceiveMessageAsync()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    var messageJson = await _streamReader.ReadLineAsync();
                    if (messageJson == null) break;

                    var message = JsonSerializer.Deserialize<Message>(messageJson);
                    MessageReceived?.Invoke(message);
                }
                catch (Exception ex)
                {
                    break;
                }
            }

            IsConnected = false;
            Console.WriteLine("Disconnected from server.");
        }

        public async Task SendMessageAsync(Message message)
        {
            var json = JsonSerializer.Serialize(message);
            await _streamWriter.WriteLineAsync(json);
        }
    }
}

