using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.DataAccesslayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManager.BusinessLayer
{
    public class DiBuilder
    {
        public static void Build(IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddEntityFrameworkSqlServer().
                AddDbContext<ProjectManagerDbContext>(option => option.UseSqlServer(config.GetSection("Database").GetSection("Connection").Value), ServiceLifetime.Scoped);

        }
    }
}
