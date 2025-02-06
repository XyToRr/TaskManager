﻿using System.Net;
using BLL.Service;

DependencyInjector.ConfigureServices();

var ipAddress = IPAddress.Parse("127.0.0.1");
var taskManagerService = new TaskManagerServer(ipAddress, 5000);

await taskManagerService.StartAsync();
