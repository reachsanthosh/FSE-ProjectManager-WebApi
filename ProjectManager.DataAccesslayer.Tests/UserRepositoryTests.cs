using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DataAccessLayer.Tests.TestHelper;
using Xunit;

namespace ProjectManager.DataAccesslayer.Tests
{
    public class UserRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private DatabaseFixture fixture;
        public UserRepositoryTests(DatabaseFixture dbFixture)
        {
            this.fixture = dbFixture;
        }

        [Fact]
        public async Task TestGetAll_ReturnsTwoProjects()
        {

            var contextOptions = new DbContextOptions<ProjectManagerDbContext>();
            var mockContext = new Mock<ProjectManagerDbContext>(contextOptions);

            var userRepository = new UserRepository(mockContext.Object, fixture.UserRepositoryLogger);

            IQueryable<User> userList = new List<User>()
            {
                new User() {UserId = 1, FirstName ="User 1 "},
                new User() {UserId = 2, FirstName ="User 2 "},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IAsyncEnumerable<User>>()
        .Setup(m => m.GetEnumerator())
        .Returns(new TestAsyncEnumerator<User>(userList.GetEnumerator()));

            mockSet.As<IQueryable<User>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<User>(userList.Provider));

            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userList.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userList.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => userList.GetEnumerator());

            mockContext.Setup(m => m.Users).Returns(mockSet.Object);
            // mockContext.SetupProperty(m => m.Tasks, mockSet.Object);

            var users = await userRepository.GetAllAsync();

            Assert.Equal(2, users.Count());
        }

        [Fact]
        public async Task TestGet_VerifyTaskName()
        {

            var contextOptions = new DbContextOptions<ProjectManagerDbContext>();
            var mockContext = new Mock<ProjectManagerDbContext>(contextOptions);

            var userRepository = new UserRepository(mockContext.Object, fixture.UserRepositoryLogger);

            IQueryable<User> ProjectsList = new List<User>()
            {
                new User() {UserId = 1, FirstName ="User 1"},
                new User() {UserId = 2, FirstName ="User 2"},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IAsyncEnumerable<User>>()
        .Setup(m => m.GetEnumerator())
        .Returns(new TestAsyncEnumerator<User>(ProjectsList.GetEnumerator()));

            mockSet.As<IQueryable<User>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<User>(ProjectsList.Provider));

            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(ProjectsList.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(ProjectsList.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => ProjectsList.GetEnumerator());

            mockContext.Setup(m => m.Users).Returns(mockSet.Object);
            // mockContext.SetupProperty(m => m.Tasks, mockSet.Object);

            var user = await userRepository.GetAsync(2);

            Assert.Equal("User 2", user.FirstName);
        }

        [Fact]
        public async Task TestInsertAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<ProjectManagerDbContext>();
            var mockContext = new Mock<ProjectManagerDbContext>(contextOptions);

            var userRepository = new UserRepository(mockContext.Object, fixture.UserRepositoryLogger);

            var project = new User() { UserId = 1, FirstName = "User 1 " };

            var mockSet = new Mock<DbSet<User>>();

            mockContext.Setup(m => m.Users).Returns(mockSet.Object);
            var result = await userRepository.InsertAsync(project);

            mockSet.Verify(m => m.Add(project), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task TestUpdateAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<ProjectManagerDbContext>();
            var mockContext = new Mock<ProjectManagerDbContext>(contextOptions);

            var userRepository = new UserRepository(mockContext.Object, fixture.UserRepositoryLogger);

            var project = new User() { UserId = 1, FirstName = "User 1"};

            var mockSet = new Mock<DbSet<User>>();

            mockContext.Setup(m => m.Users).Returns(mockSet.Object);
            var result = await userRepository.UpdateAsync(1, project);

            mockSet.Verify(m => m.Update(project), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task TestRemoveAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<ProjectManagerDbContext>();
            var mockContext = new Mock<ProjectManagerDbContext>(contextOptions);

            var userRepository = new UserRepository(mockContext.Object, fixture.UserRepositoryLogger);

            var User = new User() { UserId = 1, FirstName = "Users 1" };

            var mockSet = new Mock<DbSet<User>>();

            mockContext.Setup(m => m.Users).Returns(mockSet.Object);
            var result = await userRepository.DeleteAsync(User);

            mockSet.Verify(m => m.Remove(User), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

    }
}

