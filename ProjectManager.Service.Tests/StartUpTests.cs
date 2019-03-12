using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectManager.BusinessLayer;
using Xunit;
using ProjectManager.Service;

namespace TaskManager.Service.Tests
{
    public class StartUpTests
    {
        [Fact]
        public void TestBuild()
        {
            //var serviceCollection = new ServiceCollection();
            var configuration = new Mock<IConfiguration>();
            var serviceCollection = new ServiceCollection();
            configuration.Setup(config => config.GetSection("Database").GetSection("Connection").Value).Returns("DummyConnection");
            var startUp = new Startup(configuration.Object);
        
            startUp.ConfigureServices(serviceCollection);

            var sp = serviceCollection.BuildServiceProvider();
            var manageTask = sp.GetService<IManageTask>();
            var manageProject = sp.GetService<IManageProject>();
            var manageuser = sp.GetService<IManageUser>();
            Assert.NotNull(manageTask);
            Assert.NotNull(manageProject);
            Assert.NotNull(manageuser);
        }
       
    }

}
