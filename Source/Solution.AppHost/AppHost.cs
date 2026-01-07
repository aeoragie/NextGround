var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Ng_Web_Admin>("ng-web-admin");

builder.AddProject<Projects.Ng_Web_Portal>("ng-web-portal");

builder.AddProject<Projects.Ng_Server>("ng-server");

builder.Build().Run();
