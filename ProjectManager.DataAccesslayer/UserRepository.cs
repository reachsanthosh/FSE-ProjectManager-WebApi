using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DataAccesslayer
{
    public class UserRepository: IUserRepository
    {
        private readonly ProjectManagerDbContext projectManagerDbContext;
        private readonly ILogger<UserRepository> logger;
        public UserRepository(ProjectManagerDbContext projectManagerDbContext, ILogger<UserRepository> logger)
        {
            this.projectManagerDbContext = projectManagerDbContext;
            this.logger = logger;
        }
        public async Task<int> DeleteAsync(User entity)
        {
            projectManagerDbContext.Users.Remove(entity);

            return await projectManagerDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await projectManagerDbContext.Users.AsNoTracking<User>().ToListAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            return await projectManagerDbContext.Users.Include(userObject => userObject.Projects).Include(userObject => userObject.TaskDetails)
                .FirstOrDefaultAsync(t => t.UserId == id);
        }

        public async Task<int> InsertAsync(User entity)
        {
            projectManagerDbContext.Users.Add(entity);
            return await projectManagerDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(int id, User entity)
        {
            projectManagerDbContext.Users.Update(entity);
            return await projectManagerDbContext.SaveChangesAsync();
        }
    }
}
