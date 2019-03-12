using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManager.DataAccesslayer.Tests
{
    public class DatabaseFixture
    {
        public DatabaseFixture()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            TaskRepositoryLogger = factory.CreateLogger<TaskRepository>();
            ProjectRepositoryLogger = factory.CreateLogger<ProjectRepository>();
            UserRepositoryLogger = factory.CreateLogger<UserRepository>();
        }

        public ILogger<TaskRepository> TaskRepositoryLogger { get; private set; }
        public ILogger<ProjectRepository> ProjectRepositoryLogger { get; private set; }

        public ILogger<UserRepository> UserRepositoryLogger { get; private set; }
        public void Dispose()
        {

        }
    }
}
