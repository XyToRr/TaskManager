using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public enum MessageType
    {
        Connect,
        Disconnect,
        RegisterAccept,
        RegisterDecline,
        LoginAccept,
        LoginDecline,
        RegisterRequest,
        LoginRequest,
        ProjectListUpdate,
        ProjectCreationRequest,
        ProjectListRequest,
        TaskCreationRequest,
        FindUser,
        ProjectTasksListRequest,
        TaskAssignment,
        TaskCompletion,
        TaskStateUpdate,
        TaskUpdate,
        AddUserToProject 
    }
}
