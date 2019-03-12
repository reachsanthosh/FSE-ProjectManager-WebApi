using ProjectManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.BusinessLayer
{
   public interface IManageTask
    {
        Task<int> AddTaskAsync(TaskDetail taskDetail);
        Task<IEnumerable<TaskDetail>> GetAllTasksAsync();
        Task<TaskDetail> GetTaskAsync(int id);
        Task<int> EditTaskAsync(int id, TaskDetail taskDetail);

        bool IsTaskValidToClose(TaskDetail taskDetail);
    }
}
