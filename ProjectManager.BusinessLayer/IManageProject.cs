using ProjectManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.BusinessLayer
{
    public interface IManageProject
    {
        Task<int> AddProjectAsync(Project project);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectAsync(int id);
        Task<int> EditProjectAsync(int id, Project project);
        bool IsProjectValidToClose(Project project);

        Task<int> DeleteProjectAsync(Project project);
    }
}
