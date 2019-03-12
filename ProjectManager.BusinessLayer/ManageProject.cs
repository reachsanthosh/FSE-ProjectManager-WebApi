using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ProjectManager.DataAccesslayer;
using ProjectManager.Model;

namespace ProjectManager.BusinessLayer
{
    public class ManageProject : IManageProject
    {
        private readonly IProjectRepository projectRepository;
        private readonly ILogger<ManageProject> logger;

        public ManageProject(IProjectRepository projectRepository, ILogger<ManageProject> logger)
        {
            this.projectRepository = projectRepository;
            this.logger = logger;
        }
        public async Task<int> AddProjectAsync(Project project)
        {
            return await projectRepository.InsertAsync(project);
        }

        public async Task<int> EditProjectAsync(int id, Project project)
        {
            return await projectRepository.UpdateAsync(id, project);
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            logger.LogInformation("Getting All Projects");
            return await projectRepository.GetAllAsync();
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            logger.LogInformation($"Getting project details for the id { id }");
            return await projectRepository.GetAsync(id);
        }

        public bool IsProjectValidToClose(Project project)
        {
            logger.LogInformation("Check if project is valid to delete");
            if (project.TaskDetails.Any(taskDetail => taskDetail.ActiveStatus))
                return false;

            return true;
        }

        public async Task<int> DeleteProjectAsync(Project project)
        {
            logger.LogInformation($"Deleting project details for the id { project.ProjectId }");
            return await projectRepository.DeleteAsync(project);
        }
    }
}
