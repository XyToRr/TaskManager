﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public enum MessageType
    {
        RegisterAccept,
        RegisterDecline,
        LoginAccept,
        RegisterDecline,
        RegisterRequest,
        LoginRequest,
        RoleChange,
        ProjectCreationRequest,
        TaskCreationRequest,
        FindUser,
        TaskAssignment,
        TaskCompletion,
        TaskStateUpdate,
        TaskUpdate,
        AddUserToProject
    }
}
