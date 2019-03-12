using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectManager.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManager.BusinessLayer.Tests
{
    public class BusinessFixture : IDisposable
    {
        public BusinessFixture()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            Logger = factory.CreateLogger<ManageTask>();
            ManageProjectLogger = factory.CreateLogger<ManageProject>();
            ManageUserLogger = factory.CreateLogger<ManageUser>();
        }

        public ILogger<ManageTask> Logger { get; private set; }
         public ILogger<ManageProject> ManageProjectLogger { get; private set; }
        public ILogger<ManageUser> ManageUserLogger { get; private set; }
        public void Dispose()
        {

        }
    }
}
