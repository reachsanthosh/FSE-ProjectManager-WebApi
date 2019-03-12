using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectManager.Model;
using Xunit;
using ProjectManager.DataAccesslayer;

namespace TaskManager.DataAccessLayer.Tests
{
    public class ProjectManagerDbContextTest
    {
        [Fact]
        public void OnModelCreating_VerifyModelCreation()
        {
            var mockModel = new Mock<ModelBuilder>(new ConventionSet());
            try
            {
                var contextOptions = new DbContextOptions<ProjectManagerDbContext>();               
                var projectManagerDbContextStub = new ProjectManagerDbContextStub(contextOptions);              
                var modelBuilder = new ModelBuilder(new ConventionSet());             
                var model = new Microsoft.EntityFrameworkCore.Metadata.Internal.Model();
                var configSource = new ConfigurationSource();

                var internalModelBuilder = new InternalModelBuilder(model);             

                var taskModel = new TaskDetail();
                var entity = new EntityType("TaskModel", model, configSource);
                var internalEntityTypeBuilder = new InternalEntityTypeBuilder(entity, internalModelBuilder);
                var entityTypeBuilder = new EntityTypeBuilder<TaskDetail>(internalEntityTypeBuilder);              
                mockModel.Setup(m => m.Entity<TaskDetail>()).Returns(entityTypeBuilder);
                var property = new Property("Name", taskModel.GetType(), taskModel.GetType().GetProperty("Name"), taskModel.GetType().GetField("Name"), entity, configSource, null);
                var internalPropertyBuilder = new InternalPropertyBuilder(property, internalModelBuilder);
                var propertyBuilder = new PropertyBuilder<string>(internalPropertyBuilder);

                var userModel = new User();
                var userEntity = new EntityType("User", model, configSource);
                var userInternalEntityTypeBuilder = new InternalEntityTypeBuilder(userEntity, internalModelBuilder);
                var userEntityTypeBuilder = new EntityTypeBuilder<User>(userInternalEntityTypeBuilder);
                mockModel.Setup(m => m.Entity<User>()).Returns(userEntityTypeBuilder);
                var userProperty = new Property("FirstName", userModel.GetType(), userModel.GetType().GetProperty("FirstName"), userModel.GetType().GetField("FirstName"), entity, configSource, null);
                var userInternalPropertyBuilder = new InternalPropertyBuilder(userProperty, internalModelBuilder);
                var userPropertyBuilder = new PropertyBuilder<string>(userInternalPropertyBuilder);

                var projectModel = new Project();
                var projectEntity = new EntityType("Project", model, configSource);
                var projectInternalEntityTypeBuilder = new InternalEntityTypeBuilder(projectEntity, internalModelBuilder);
                var projectEntityTypeBuilder = new EntityTypeBuilder<Project>(projectInternalEntityTypeBuilder);
                mockModel.Setup(m => m.Entity<Project>()).Returns(projectEntityTypeBuilder);
                var projectProperty = new Property("FirstName", projectModel.GetType(), projectModel.GetType().GetProperty("FirstName"), projectModel.GetType().GetField("FirstName"), entity, configSource, null);
                var projectInternalPropertyBuilder = new InternalPropertyBuilder(projectProperty, internalModelBuilder);
                var projectPropertyBuilder = new PropertyBuilder<string>(projectInternalPropertyBuilder);

                projectManagerDbContextStub.TestModelCreation(modelBuilder);                
            }
            catch (Exception ex)
            {
                mockModel.Verify(m => m.Entity<TaskDetail>().HasKey("Id"), Times.Once);
                mockModel.Verify(m => m.Entity<User>().HasKey("UserId"), Times.Once);
                mockModel.Verify(m => m.Entity<Project>().HasKey("ProjectId"), Times.Once);
                Assert.NotNull(ex);
            }
        }
    }

    public class ProjectManagerDbContextStub : ProjectManagerDbContext
    {
        public ProjectManagerDbContextStub(DbContextOptions options):base(options)
        {

        }
        public void TestModelCreation(ModelBuilder model)
        {
            OnModelCreating(model);           
        }
    }
}
