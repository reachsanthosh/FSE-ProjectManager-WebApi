using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataAccesslayer
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectManagerDbContext projectManagerDbContext;
        private readonly ILogger<ProjectRepository> logger;
        public ProjectRepository(ProjectManagerDbContext projectManagerDbContext, ILogger<ProjectRepository> logger)
        {
            this.projectManagerDbContext = projectManagerDbContext;          
            this.logger = logger;
        }

        public async Task<int> DeleteAsync(Project entity)
        {
            projectManagerDbContext.Projects.Remove(entity);

            return await projectManagerDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await projectManagerDbContext.Projects.Include(project => project.TaskDetails).Include(project => project.UserDetail)
                .AsNoTracking<Project>().ToListAsync();
        }

        public async Task<Project> GetAsync(int id)
        {
            return await projectManagerDbContext.Projects.              
                Include(project => project.TaskDetails).Include(project => project.UserDetail).FirstOrDefaultAsync(t => t.ProjectId == id);
        }

        public async Task<int> InsertAsync(Project entity)
        {
            entity.UserDetail = null;            
            projectManagerDbContext.Projects.Add(entity);
            return await projectManagerDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(int id, Project entity)
        {

            projectManagerDbContext.Projects.Update(entity);
            return await projectManagerDbContext.SaveChangesAsync();
        }
    }
}
