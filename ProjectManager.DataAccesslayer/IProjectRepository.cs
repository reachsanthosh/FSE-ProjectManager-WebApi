using ProjectManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataAccesslayer
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project> GetAsync(int id);
        Task<int> InsertAsync(Project entity);
        Task<int> UpdateAsync(int id, Project entity);
        Task<int> DeleteAsync(Project entity);
    }
}
