using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManager.BusinessLayer;
using ProjectManager.Model;

namespace ProjectManager.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly IManageUser manageUser;
        private readonly ILogger<UsersController> logger;

        public UsersController(IManageUser manageUser, ILogger<UsersController> logger)
        {
            this.manageUser = manageUser;
            this.logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                logger.LogInformation("Get All Users");

                return Ok(await manageUser.GetAllUsersAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }

        // GET: api/Tasks/5
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                logger.LogInformation($"Getting user details for {id}");

                return Ok(await manageUser.GetUserAsync(id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]User user)
        {
            try
            {
                if (user == null)
                {
                    logger.LogInformation($"User is null.  Provide valid user details.");
                    return BadRequest($"User is null.  Provide valid user details.");
                }

                await manageUser.AddUserAsync(user);

                logger.LogInformation($"User has been added Successfully and the new user id is { user.UserId }");

                return Ok(user.UserId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]User user)
        {
            try
            {
                logger.LogInformation($"Updating user {id}");
                if (user == null || id != user.UserId)
                {
                    logger.LogInformation("Invalid user to edit");
                    return BadRequest("Invalid user to edit.");
                }
                
                await manageUser.EditUserAsync(id, user);
                logger.LogInformation($"User has been updated successfully for the user id { user.UserId } ");
                return Ok(user.UserId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var user = await manageUser.GetUserAsync(id);
                if (!manageUser.IsUserValidToDelete(user))
                {
                    logger.LogInformation("You can not delete as the user have association with Project/Task");
                    return BadRequest("You can not delete as the user have association with Project/Task");
                }

                await manageUser.DeleteUserAsync(user);

                return Ok(user.UserId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }          
        }
    }
}