using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ClientModels
{
    public class ProjectOnClient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public int TaskCount { get; set; }
        public string CreatedDate { get; set; }
    }
}
