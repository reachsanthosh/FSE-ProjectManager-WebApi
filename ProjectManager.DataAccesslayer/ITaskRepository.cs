using ProjectManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataAccesslayer
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskDetail>> GetAllAsync();
        Task<TaskDetail> GetAsync(int id);
        Task<int> InsertAsync(TaskDetail entity);
        Task<int> UpdateAsync(int id, TaskDetail entity);
        Task<int> DeleteAsync(TaskDetail entity);
    }
}
