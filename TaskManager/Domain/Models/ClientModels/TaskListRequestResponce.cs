using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ClientModels
{
    public class TaskListRequestResponce
    {
        public Role Role { get; set; }
        public List<ClientTaskInfo> Tasks { get; set; }
    }
}
