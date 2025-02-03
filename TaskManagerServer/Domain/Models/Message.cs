using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Message
    {
        public string Content { get; set; }
        public MessageType MessageType { get; set; }
    }
}
