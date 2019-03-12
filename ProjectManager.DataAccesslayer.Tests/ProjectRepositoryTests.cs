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
    public class ProjectRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private DatabaseFixture fixture;
        public ProjectRepositoryTests(DatabaseFixture dbFixture)
        {
            this.fixture = dbFixture;
        }

        [Fact]       
        public async Task TestGetAll_ReturnsTwoProjects()
        {

            var contextOptions = new DbContextOptions<ProjectManagerDbContext>();
            var mockContext = new Mock<ProjectManagerDbContext>(contextOptions);

            var projectRepository = new ProjectRepository(mockContext.Object, fixture.ProjectRepositoryLogger);

            IQueryable<Project> projectList = new List<Project>()
            {
                new Project() {ProjectId = 1, ProjectName ="Project 1 ", Priority = 10},
                new Project() {ProjectId = 2, ProjectName ="Project 2 ", Priority = 20},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Project>>();

            mockSet.As<IAsyncEnumerable<Project>>()
        .Setup(m => m.GetEnumerator())
        .Returns(new TestAsyncEnumerator<Project>(projectList.GetEnumerator()));

            mockSet.As<IQueryable<Project>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Project>(projectList.Provider));

            mockSet.As<IQueryable<Project>>().Setup(m => m.Expression).Returns(projectList.Expression);
            mockSet.As<IQueryable<Project>>().Setup(m => m.ElementType).Returns(projectList.ElementType);
            mockSet.As<IQueryable<Project>>().Setup(m => m.GetEnumerator()).Returns(() => projectList.GetEnumerator());

            mockContext.Setup(m => m.Projects).Returns(mockSet.Object);
            // mockContext.SetupProperty(m => m.Tasks, mockSet.Object);

            var Projects = await projectRepository.GetAllAsync();

            Assert.Equal(2, Projects.Count());
        }

        [Fact]
        public async Task TestGet_VerifyTaskName()
        {

            var contextOptions = new DbContextOptions<ProjectManagerDbContext>();
            var mockContext = new Mock<ProjectManagerDbContext>(contextOptions);

            var projectRepository = new ProjectRepository(mockContext.Object, fixture.ProjectRepositoryLogger);

            IQueryable<Project> ProjectsList = new List<Project>()
            {
                new Project() {ProjectId = 1, ProjectName ="Project 1", Priority = 10},
                new Project() {ProjectId = 2, ProjectName ="Project 2", Priority = 20},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Project>>();

            mockSet.As<IAsyncEnumerable<Project>>()
        .Setup(m => m.GetEnumerator())
        .Returns(new TestAsyncEnumerator<Project>(ProjectsList.GetEnumerator()));

            mockSet.As<IQueryable<Project>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Project>(ProjectsList.Provider));

            mockSet.As<IQueryable<Project>>().Setup(m => m.Expression).Returns(ProjectsList.Expression);
            mockSet.As<IQueryable<Project>>().Setup(m => m.ElementType).Returns(ProjectsList.ElementType);
            mockSet.As<IQueryable<Project>>().Setup(m => m.GetEnumerator()).Returns(() => ProjectsList.GetEnumerator());

            mockContext.Setup(m => m.Projects).Returns(mockSet.Object);
            // mockContext.SetupProperty(m => m.Tasks, mockSet.Object);

            var project = await projectRepository.GetAsync(2);

            Assert.Equal("Project 2", project.ProjectName);
        }

        [Fact]
        public async Task TestInsertAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<ProjectManagerDbContext>();
            var mockContext = new Mock<ProjectManagerDbContext>(contextOptions);

            var projectRepository = new ProjectRepository(mockContext.Object, fixture.ProjectRepositoryLogger);

            var project = new Project() { ProjectId = 1, ProjectName = "Project 1 ", Priority = 10 };

            var mockSet = new Mock<DbSet<Project>>();

            mockContext.Setup(m => m.Projects).Returns(mockSet.Object);
            var result = await projectRepository.InsertAsync(project);

            mockSet.Verify(m => m.Add(project), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task TestUpdateAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<ProjectManagerDbContext>();
            var mockContext = new Mock<ProjectManagerDbContext>(contextOptions);

            var projectRepository = new ProjectRepository(mockContext.Object, fixture.ProjectRepositoryLogger);

            var project = new Project() { ProjectId = 1, ProjectName = "Project 1", Priority = 10 };

            var mockSet = new Mock<DbSet<Project>>();

            mockContext.Setup(m => m.Projects).Returns(mockSet.Object);
            var result = await projectRepository.UpdateAsync(1, project);

            mockSet.Verify(m => m.Update(project), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task TestRemoveAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<ProjectManagerDbContext>();
            var mockContext = new Mock<ProjectManagerDbContext>(contextOptions);

            var projectRepository = new ProjectRepository(mockContext.Object, fixture.ProjectRepositoryLogger);

            var Project = new Project() { ProjectId = 1, ProjectName = "Projects 1", Priority = 10 };

            var mockSet = new Mock<DbSet<Project>>();

            mockContext.Setup(m => m.Projects).Returns(mockSet.Object);
            var result = await projectRepository.DeleteAsync(Project);

            mockSet.Verify(m => m.Remove(Project), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

    }
}
