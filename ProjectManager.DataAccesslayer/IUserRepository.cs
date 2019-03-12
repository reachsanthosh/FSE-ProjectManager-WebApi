using ProjectManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataAccesslayer
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetAsync(int id);
        Task<int> InsertAsync(User entity);
        Task<int> UpdateAsync(int id, User entity);
        Task<int> DeleteAsync(User entity);
    }
}
