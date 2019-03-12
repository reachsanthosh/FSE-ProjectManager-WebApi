using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectManager.BusinessLayer;
using ProjectManager.Model;
using ProjectManager.Service.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjectManager.Service.Tests
{
    public class ProjectsControllerTests : IClassFixture<ServiceFixture>
    {
        private ServiceFixture fixture;
        public ProjectsControllerTests(ServiceFixture serviceFixture)
        {
            this.fixture = serviceFixture;
        }

        [Fact]
        public async Task TestGetAllAsync_VerifyServiceReturnOkStatus()
        {
            var mockManageProject = new Mock<IManageProject>();
            var taskRepository = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);

            var projectList = new List<Project>()
            {
                new Project() {ProjectId = 1, ProjectName ="Project 1 ", Priority = 10},
                new Project() {ProjectId = 2, ProjectName ="Project 2 ", Priority = 20},
            };

            mockManageProject.Setup(manage => manage.GetAllProjectsAsync()).Returns(Task.FromResult<IEnumerable<Project>>(projectList));

            var statusResult = await taskRepository.GetAllAsync();

            Assert.NotNull(statusResult as OkObjectResult);

            var projectResult = (statusResult as OkObjectResult).Value as List<Project>;
            Assert.Equal(2, projectResult.Count);
        }

        [Fact]
        public async Task TestGetAllAsync_WhenManageProjectThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);

            mockManageProject.Setup(manage => manage.GetAllProjectsAsync()).Throws(new Exception());

            var statusResult = await projectController.GetAllAsync();

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestGetAsync_VerifyServiceReturnOkStatusAndCheckProjectDetails()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);

            var project = new Project() { ProjectId = 1, ProjectName = "Project 1", Priority = 10 };

            mockManageProject.Setup(manage => manage.GetProjectAsync(1)).Returns(Task.FromResult<Project>(project));

            var statusResult = await projectController.GetAsync(1);

            Assert.NotNull(statusResult as OkObjectResult);

            var projectResult = (statusResult as OkObjectResult).Value as Project;
            Assert.Equal("Project 1", projectResult.ProjectName);
            Assert.Equal(10, projectResult.Priority);
        }


        [Fact]
        public async Task TestGetAsync_WhenManageProjectThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);

            mockManageProject.Setup(manage => manage.GetProjectAsync(1)).Throws(new Exception());

            var statusResult = await projectController.GetAsync(1);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestPostAsync_VerifyServiceReturnOkStatusAndCheckProjectId()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);

            var project = new Project() { ProjectId = 1001, ProjectName = "Project 1", Priority = 10 };

            mockManageProject.Setup(manage => manage.AddProjectAsync(project)).Returns(Task.FromResult<int>(1001));

            var statusResult = await projectController.PostAsync(project);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal(1001, (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPostAsync_PassNullAndVerifyServiceReturnBadRequest()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);

            var statusResult = await projectController.PostAsync(null);

            Assert.NotNull(statusResult as BadRequestResult);
        }

        [Fact]
        public async Task TestPostAsync_WhenManageProjectThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);
            var project = new Project() { ProjectId = 1001, ProjectName = "Project 1", Priority = 10 };

            mockManageProject.Setup(manage => manage.AddProjectAsync(project)).Throws(new Exception());

            var statusResult = await projectController.PostAsync(project);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }


        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnOkStatusAndCheckServiceResponse()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);

            var project = new Project() { ProjectId = 1001, ProjectName = "Project 1", Priority = 10, ActiveStatus = true };

            mockManageProject.Setup(manage => manage.EditProjectAsync(1001, project)).Returns(Task.FromResult<int>(1001));

            var statusResult = await projectController.PutAsync(1001, project);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal(project.ProjectId, (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenProjectIsNull()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);

            var statusResult = await projectController.PutAsync(1001, null);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("Invalid project to edit.", (statusResult as BadRequestObjectResult).Value);
        }


        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenProjectIdIsInvalid()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);
            var project = new Project() { ProjectId = 1001, ProjectName = "Project 1", Priority = 10 };
            var statusResult = await projectController.PutAsync(1002, project);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("Invalid project to edit.", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenProjectIsNotValidToClose()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);
            var project = new Project() { ProjectId = 1001, ProjectName = "Task 1", Priority = 10, ActiveStatus = false };
            mockManageProject.Setup(manage => manage.IsProjectValidToClose(project)).Returns(false);
            var statusResult = await projectController.PutAsync(1001, project);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("You can not close this project as the project has association with task", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnOkStatusWhenProjectIsValidToClose()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);

            var project = new Project() { ProjectId = 1001, ProjectName = "Project 1", Priority = 10, ActiveStatus = false };

            mockManageProject.Setup(manage => manage.IsProjectValidToClose(project)).Returns(true);

            mockManageProject.Setup(manage => manage.EditProjectAsync(1001, project)).Returns(Task.FromResult<int>(1001));

            var statusResult = await projectController.PutAsync(1001, project);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal(project.ProjectId, (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_WhenManageProjectThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);
            var project = new Project() { ProjectId = 1001, ProjectName = "Project 1", Priority = 10, ActiveStatus = true };
            mockManageProject.Setup(manage => manage.EditProjectAsync(1001, project)).Throws(new Exception());

            var statusResult = await projectController.PutAsync(1001, project);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestDeleteAsync_WhenIsProjectValidToCloseReturnTrueVerifyServiceReturnOkStatus()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);
            var project = new Project() { ProjectId = 1001, ProjectName = "Project 1", Priority = 10, ActiveStatus = false };
            mockManageProject.Setup(manage => manage.GetProjectAsync(1001)).Returns(Task.FromResult(project));
            mockManageProject.Setup(manage => manage.IsProjectValidToClose(project)).Returns(true);
            var statusResult = await projectController.DeleteAsync(1001);

            Assert.NotNull(statusResult as OkObjectResult);
        }

        [Fact]
        public async Task TestDeleteAsync_WhenIsProjectValidToCloseReturnFalseVerifyServiceReturnOkStatus()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);
            var project = new Project() { ProjectId = 1001, ProjectName = "Project 1", Priority = 10, ActiveStatus = false };
            mockManageProject.Setup(manage => manage.GetProjectAsync(1001)).Returns(Task.FromResult(project));
            mockManageProject.Setup(manage => manage.IsProjectValidToClose(project)).Returns(false);
            var statusResult = await projectController.DeleteAsync(1001);

            Assert.NotNull(statusResult as BadRequestObjectResult);
        }

        [Fact]
        public async Task TestDeleteAsync_WhenIsProjectValidToCloseReturnExceptionVerifyServiceReturnInternalServerError()
        {
            var mockManageProject = new Mock<IManageProject>();
            var projectController = new ProjectsController(mockManageProject.Object, fixture.projectControllerLogger);
            var project = new Project() { ProjectId = 1001, ProjectName = "Project 1", Priority = 10, ActiveStatus = false };
            mockManageProject.Setup(manage => manage.GetProjectAsync(1001)).Returns(Task.FromResult(project));
            mockManageProject.Setup(manage => manage.IsProjectValidToClose(project)).Throws(new Exception());
            var statusResult = await projectController.DeleteAsync(1001);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

    }
}
