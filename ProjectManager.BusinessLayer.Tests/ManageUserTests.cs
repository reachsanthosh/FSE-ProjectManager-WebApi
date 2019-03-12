using Moq;
using ProjectManager.BusinessLayer;
using ProjectManager.DataAccesslayer;
using ProjectManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.BusinessLayer.Tests;
using Xunit;

namespace ProjectManager.BusinessLayer.Tests
{
   public class ManageUserTests : IClassFixture<BusinessFixture>
    {
        private BusinessFixture fixture;
        public ManageUserTests(BusinessFixture dbFixture)
        {
            this.fixture = dbFixture;
        }

        [Fact]
        public async Task TestAddUserAsync_VerifyInsertAsyncCalledOnce()
        {
            var mockRepository = new Mock<IUserRepository>();
            var manageUser = new ManageUser(mockRepository.Object, fixture.ManageUserLogger);
            var user = new User();
            var result = await manageUser.AddUserAsync(user);

            mockRepository.Verify(r => r.InsertAsync(user), Times.Once);
        }

        [Fact]
        public async Task TestEditUserAsync_VerifyUpdateAsyncCalledOnce()
        {
            var mockRepository = new Mock<IUserRepository>();
            var manageUser = new ManageUser(mockRepository.Object, fixture.ManageUserLogger);
            var user = new User();
            var result = await manageUser.EditUserAsync(10, user);

            mockRepository.Verify(r => r.UpdateAsync(10, user), Times.Once);
        }

        [Fact]
        public async Task TestGetAllUsersAsync_VerifyGetAllAsyncCalledOnce()
        {
            var mockRepository = new Mock<IUserRepository>();
            var manageUser = new ManageUser(mockRepository.Object, fixture.ManageUserLogger);

            var result = await manageUser.GetAllUsersAsync();

            mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task TestGetUserAsync_VerifyGetAsyncCalledOnce()
        {
            var mockRepository = new Mock<IUserRepository>();
            var manageUser = new ManageUser(mockRepository.Object, fixture.ManageUserLogger);

            var result = await manageUser.GetUserAsync(10);

            mockRepository.Verify(r => r.GetAsync(10), Times.Once);
        }

        [Fact]
        public async Task TestDeleteProjectAsync_VerifyGetAsyncCalledOnce()
        {
            var mockRepository = new Mock<IUserRepository>();
            var manageUser = new ManageUser(mockRepository.Object, fixture.ManageUserLogger);
            var user = new User();
            var result = await manageUser.DeleteUserAsync(user);

            mockRepository.Verify(r => r.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public void TestIsUserValidToDelete_ReturnFalseWhenUserContainsActiveTasks()
        {
            var mockRepository = new Mock<IUserRepository>();
            var manageUser = new ManageUser(mockRepository.Object, fixture.ManageUserLogger);

            var taskDetail = new TaskDetail() { Id = 1, Name = "Task 1", Priority = 20, ActiveStatus = true };

            var taskDetailsList = new List<TaskDetail>()
            {
                taskDetail,
                new TaskDetail() {Id = 2, Name ="Task 2 ", Priority = 20},
            };

            var user = new User() { UserId = 1, FirstName = "User 1", TaskDetails = taskDetailsList, Projects = new List<Project>() };

            var result = manageUser.IsUserValidToDelete(user);

            Assert.False(result);
        }

        [Fact]
        public void TestIsUserValidToDelete_ReturnTrueWhenUserContainsCompletedTasks()
        {
            var mockRepository = new Mock<IUserRepository>();
            var manageUser = new ManageUser(mockRepository.Object, fixture.ManageUserLogger);

            var taskDetail = new TaskDetail() { Id = 1, Name = "Task 1", Priority = 20, ActiveStatus = false };

            var taskDetailsList = new List<TaskDetail>()
            {
                taskDetail,
                new TaskDetail() {Id = 2, Name ="Task 2 ", Priority = 20},
            };

            var user = new User() { UserId = 1, FirstName = "User 1", TaskDetails = taskDetailsList, Projects = new List<Project>() };

            var result = manageUser.IsUserValidToDelete(user);

            Assert.True(result);
        }


        [Fact]
        public void TesIsUserValidToDelete_ReturnFalseWhenUserContainsActiveProjects()
        {
            var mockRepository = new Mock<IUserRepository>();
            var manageUser = new ManageUser(mockRepository.Object, fixture.ManageUserLogger);

            var project = new Project() { ProjectId = 1, ProjectName = "Project 1", Priority = 20 };

            var projectList = new List<Project>()
            {
               project
            };

            var user = new User() { UserId = 1, FirstName = "User 1", Projects = projectList, TaskDetails = new List<TaskDetail>() };

            var result = manageUser.IsUserValidToDelete(user);

            Assert.False(result);
        }

        [Fact]
        public void TestIsUserValidToDelete_ReturnFalseWhenUserContainsActiveProjectsOrTasks()
        {
            var mockRepository = new Mock<IUserRepository>();
            var manageUser = new ManageUser(mockRepository.Object, fixture.ManageUserLogger);

            var project = new Project() { ProjectId = 1, ProjectName = "Project 1", Priority = 20 };

            var projectList = new List<Project>()
            {
               project
            };

            var taskDetail = new TaskDetail() { Id = 1, Name = "Task 1", Priority = 20, ActiveStatus = true };

            var taskDetailsList = new List<TaskDetail>()
            {
                taskDetail,
                new TaskDetail() {Id = 2, Name ="Task 2 ", Priority = 20},
            };           

            var user = new User() { UserId = 1, FirstName = "User 1", Projects = projectList, TaskDetails = taskDetailsList };

            var result = manageUser.IsUserValidToDelete(user);

            Assert.False(result);
        }
    }
}
