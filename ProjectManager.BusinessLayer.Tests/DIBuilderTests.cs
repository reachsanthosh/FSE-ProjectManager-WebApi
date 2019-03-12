using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProjectManager.BusinessLayer;
using ProjectManager.DataAccesslayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace ProjectManager.BusinessLayer.Tests
{
    public class DIBuilderTests
    {
        [Fact]
        public void TestBuild_VerifyDependendencyObjectsAreNotNull()
        {
            var serviceCollection = new ServiceCollection();
            var configuration = new Mock<IConfiguration>();

            configuration.Setup(config => config.GetSection("Database").GetSection("Connection").Value).Returns("DummyConnection");
            DiBuilder.Build(serviceCollection, configuration.Object);
            var sp = serviceCollection.BuildServiceProvider();
            var result = sp.GetService<ITaskRepository>();
            Assert.NotNull(result);
            var dbContext = sp.GetService<ProjectManagerDbContext>();
            Assert.NotNull(dbContext);
        }
    }
}
