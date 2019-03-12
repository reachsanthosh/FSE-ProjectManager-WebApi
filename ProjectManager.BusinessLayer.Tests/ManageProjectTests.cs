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
    public class ManageProjectTests : IClassFixture<BusinessFixture>
    {
        private BusinessFixture fixture;
        public ManageProjectTests(BusinessFixture dbFixture)
        {
            this.fixture = dbFixture;
        }

        [Fact]
        public async Task TestAddProjectAsync_VerifyInsertAsyncCalledOnce()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var manageProject = new ManageProject(mockRepository.Object, fixture.ManageProjectLogger);
            var project = new Project();
            var result = await manageProject.AddProjectAsync(project);

            mockRepository.Verify(r => r.InsertAsync(project), Times.Once);
        }

        [Fact]
        public async Task TestEditProjectAsync_VerifyUpdateAsyncCalledOnce()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var manageProject = new ManageProject(mockRepository.Object, fixture.ManageProjectLogger);
            var project = new Project();
            var result = await manageProject.EditProjectAsync(10, project);

            mockRepository.Verify(r => r.UpdateAsync(10, project), Times.Once);
        }

        [Fact]
        public async Task TestGetAllProjectsAsync_VerifyGetAllAsyncCalledOnce()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var manageProject = new ManageProject(mockRepository.Object, fixture.ManageProjectLogger);

            var result = await manageProject.GetAllProjectsAsync();

            mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task TestGetProjectAsync_VerifyGetAsyncCalledOnce()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var manageProject = new ManageProject(mockRepository.Object, fixture.ManageProjectLogger);

            var result = await manageProject.GetProjectAsync(10);

            mockRepository.Verify(r => r.GetAsync(10), Times.Once);
        }

        [Fact]
        public async Task TestDeleteProjectAsync_VerifyGetAsyncCalledOnce()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var manageProject = new ManageProject(mockRepository.Object, fixture.ManageProjectLogger);
            var project = new Project();
            var result = await manageProject.DeleteProjectAsync(project);

            mockRepository.Verify(r => r.DeleteAsync(project), Times.Once);
        }

        [Fact]
        public void TestIsProjectValidToClose_ReturnFalseWhenProjectContainsActiveTasks()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var manageProject = new ManageProject(mockRepository.Object, fixture.ManageProjectLogger);
          
            var taskDetail = new TaskDetail() { Id = 1, Name = "Task 1", Priority = 20, ActiveStatus = true };

            var taskDetailsList = new List<TaskDetail>()
            {
                taskDetail,
                new TaskDetail() {Id = 2, Name ="Task 2 ", Priority = 20},
            };

            var project = new Project() { ProjectId = 1, ProjectName = "Project 1", Priority = 20, TaskDetails = taskDetailsList };
         
            var result = manageProject.IsProjectValidToClose(project);

            Assert.False(result);
        }

        [Fact]
        public void TestIsProjectValidToClose_ReturnTrueWhenProjectContainsCompletedTasks()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var manageProject = new ManageProject(mockRepository.Object, fixture.ManageProjectLogger);

            var taskDetail = new TaskDetail() { Id = 1, Name = "Task 1", Priority = 20, ActiveStatus = false };

            var taskDetailsList = new List<TaskDetail>()
            {
                taskDetail,
                new TaskDetail() {Id = 2, Name ="Task 2 ", Priority = 20},
            };

            var project = new Project() { ProjectId = 1, ProjectName = "Project 1", Priority = 20, TaskDetails = taskDetailsList };

            var result = manageProject.IsProjectValidToClose(project);

            Assert.True(result);
        }

        [Fact]
        public void TestIsProjectValidToClose_ReturnTrueWhenProjectNotContainsAnyTasks()
        {
            var mockRepository = new Mock<IProjectRepository>();
            var manageProject = new ManageProject(mockRepository.Object, fixture.ManageProjectLogger);           

            var project = new Project() { ProjectId = 1, ProjectName = "Project 1", Priority = 20, TaskDetails = new List<TaskDetail>() };

            var result = manageProject.IsProjectValidToClose(project);

            Assert.True(result);
        }
    }
}
