using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.DataAccesslayer;
using ProjectManager.Service;

namespace ProjectManager.Load.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void BuildOtherLayerServices(IServiceCollection services)
        {
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddDbContext<ProjectManagerDbContext>(options =>
                options.UseInMemoryDatabase("projectmanager_test_db"));

        }
    }
}
