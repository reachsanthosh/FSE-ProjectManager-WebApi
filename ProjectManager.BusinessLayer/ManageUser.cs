using Microsoft.Extensions.Logging;
using ProjectManager.DataAccesslayer;
using ProjectManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.BusinessLayer
{
    public class ManageUser : IManageUser
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<ManageUser> logger;

        public ManageUser(IUserRepository userRepository, ILogger<ManageUser> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<int> AddUserAsync(User user)
        {
            return await userRepository.InsertAsync(user);
        }

        public async Task<int> EditUserAsync(int id, User userDetail)
        {
            return await userRepository.UpdateAsync(id, userDetail);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            logger.LogInformation("Getting All Users");
            return await userRepository.GetAllAsync();
        }

        public async Task<User> GetUserAsync(int id)
        {
            logger.LogInformation($"Getting user details for the id { id }");
            return await userRepository.GetAsync(id);
        }

        public async Task<int> DeleteUserAsync(User user)
        {
            logger.LogInformation($"Deleting user details for the id { user.UserId }");
            return await userRepository.DeleteAsync(user);
        }

        public bool IsUserValidToDelete(User userDetail)
        {
            logger.LogInformation("Check if user is valid to delete");
            if (userDetail.Projects.Count > 0 || userDetail.TaskDetails.Any(taskDetail => taskDetail.ActiveStatus))
                return false;

            return true;
        }
    }
}
