using ProjectManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.BusinessLayer
{
    public interface IManageUser
    {
        Task<int> AddUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<int> EditUserAsync(int id, User user);
        Task<int> DeleteUserAsync(User user);
        bool IsUserValidToDelete(User user);
    }
}
