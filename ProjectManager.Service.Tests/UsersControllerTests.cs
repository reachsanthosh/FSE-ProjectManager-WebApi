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
    public class UsersControllerTests : IClassFixture<ServiceFixture>
    {
        private ServiceFixture fixture;
        public UsersControllerTests(ServiceFixture serviceFixture)
        {
            this.fixture = serviceFixture;
        }

        [Fact]
        public async Task TestGetAllAsync_VerifyServiceReturnOkStatus()
        {
            var mockManageUser = new Mock<IManageUser>();
            var taskRepository = new UsersController(mockManageUser.Object, fixture.userControllerLogger);

            var userList = new List<User>()
            {
                new User() {UserId = 1, FirstName ="User 1", EmployeeId = 10},
                new User() {UserId = 2, FirstName ="User 2", EmployeeId = 20},
            };

            mockManageUser.Setup(manage => manage.GetAllUsersAsync()).Returns(Task.FromResult<IEnumerable<User>>(userList));

            var statusResult = await taskRepository.GetAllAsync();

            Assert.NotNull(statusResult as OkObjectResult);

            var projectResult = (statusResult as OkObjectResult).Value as List<User>;
            Assert.Equal(2, projectResult.Count);
        }

        [Fact]
        public async Task TestGetAllAsync_WhenManageUserThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);

            mockManageUser.Setup(manage => manage.GetAllUsersAsync()).Throws(new Exception());

            var statusResult = await userController.GetAllAsync();

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestGetAsync_VerifyServiceReturnOkStatusAndCheckUserDetails()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);

            var user = new User() { UserId = 1, FirstName = "User 1", EmployeeId = 10 };

            mockManageUser.Setup(manage => manage.GetUserAsync(1)).Returns(Task.FromResult<User>(user));

            var statusResult = await userController.GetAsync(1);

            Assert.NotNull(statusResult as OkObjectResult);

            var userResult = (statusResult as OkObjectResult).Value as User;
            Assert.Equal("User 1", userResult.FirstName);
            Assert.Equal(10, userResult.EmployeeId);
        }


        [Fact]
        public async Task TestGetAsync_WhenManageUserThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);

            mockManageUser.Setup(manage => manage.GetUserAsync(1)).Throws(new Exception());

            var statusResult = await userController.GetAsync(1);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestPostAsync_VerifyServiceReturnOkStatusAndCheckUserId()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);

            var user = new User() { UserId = 1001, FirstName = "User 1", EmployeeId = 10 };

            mockManageUser.Setup(manage => manage.AddUserAsync(user)).Returns(Task.FromResult<int>(1001));

            var statusResult = await userController.PostAsync(user);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal(1001, (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPostAsync_PassNullAndVerifyServiceReturnBadRequest()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);

            var statusResult = await userController.PostAsync(null);

            Assert.NotNull(statusResult as BadRequestObjectResult);
        }

        [Fact]
        public async Task TestPostAsync_WhenManageUserThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);
            var user = new User() { UserId = 1001, FirstName = "User 1", EmployeeId = 10 };

            mockManageUser.Setup(manage => manage.AddUserAsync(user)).Throws(new Exception());

            var statusResult = await userController.PostAsync(user);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnOkStatusAndCheckServiceResponse()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);

            var user = new User() { UserId = 1001, FirstName = "User 1", EmployeeId = 10};

            mockManageUser.Setup(manage => manage.EditUserAsync(1001, user)).Returns(Task.FromResult<int>(1001));

            var statusResult = await userController.PutAsync(1001, user);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal(user.UserId, (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenUserIsNull()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);

            var statusResult = await userController.PutAsync(1001, null);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("Invalid user to edit.", (statusResult as BadRequestObjectResult).Value);
        }


        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenUserIdIsInvalid()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);
            var user = new User() { UserId = 1001, FirstName = "User 1", EmployeeId = 10 };
            var statusResult = await userController.PutAsync(1002, user);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("Invalid user to edit.", (statusResult as BadRequestObjectResult).Value);
        }

       

        [Fact]
        public async Task TestPutAsync_WhenManageUserThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);
            var user = new User() { UserId = 1001, FirstName = "User 1", EmployeeId = 10 };
            mockManageUser.Setup(manage => manage.EditUserAsync(1001, user)).Throws(new Exception());

            var statusResult = await userController.PutAsync(1001, user);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestDeleteAsync_WhenIsUserValidToCloseReturnTrueVerifyServiceReturnOkStatus()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);
            var user = new User() { UserId = 1001, FirstName = "User 1", EmployeeId = 10};
            mockManageUser.Setup(manage => manage.GetUserAsync(1001)).Returns(Task.FromResult(user));
            mockManageUser.Setup(manage => manage.IsUserValidToDelete(user)).Returns(true);
            var statusResult = await userController.DeleteAsync(1001);

            Assert.NotNull(statusResult as OkObjectResult);
        }

        [Fact]
        public async Task TestDeleteAsync_WhenIsUserValidToCloseReturnFalseVerifyServiceReturnOkStatus()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);
            var user = new User() { UserId = 1001, FirstName = "User 1", EmployeeId = 10 };
            mockManageUser.Setup(manage => manage.GetUserAsync(1001)).Returns(Task.FromResult(user));
            mockManageUser.Setup(manage => manage.IsUserValidToDelete(user)).Returns(false);
            var statusResult = await userController.DeleteAsync(1001);

            Assert.NotNull(statusResult as BadRequestObjectResult);
        }

        [Fact]
        public async Task TestDeleteAsync_WhenIsUserValidToCloseReturnExceptionVerifyServiceReturnInternalServerError()
        {
            var mockManageUser = new Mock<IManageUser>();
            var userController = new UsersController(mockManageUser.Object, fixture.userControllerLogger);
            var user = new User() { UserId = 1001, FirstName = "User 1", EmployeeId = 10 };
            mockManageUser.Setup(manage => manage.GetUserAsync(1001)).Returns(Task.FromResult(user));
            mockManageUser.Setup(manage => manage.IsUserValidToDelete(user)).Throws(new Exception());
            var statusResult = await userController.DeleteAsync(1001);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }
    }
}
