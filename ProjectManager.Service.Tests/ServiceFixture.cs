using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectManager.Service.Controllers;
using System;
using System.Collections.Generic;
using System.Text;


namespace ProjectManager.Service.Tests
{
    public class ServiceFixture : IDisposable
    {
        public ServiceFixture()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            taskControllerLogger = factory.CreateLogger<TasksController>();
            projectControllerLogger = factory.CreateLogger<ProjectsController>();
            userControllerLogger = factory.CreateLogger<UsersController>();
        }

        public ILogger<TasksController> taskControllerLogger { get; private set; }
        public ILogger<ProjectsController> projectControllerLogger { get; private set; }
        public ILogger<UsersController> userControllerLogger { get; private set; }
        public void Dispose()
        {

        }
    }
}
