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
    [Route("api/Projects")]
    public class ProjectsController : Controller
    {
        private readonly IManageProject manageProject;
        private readonly ILogger<ProjectsController> logger;

        public ProjectsController(IManageProject manageProject, ILogger<ProjectsController> logger)
        {
            this.manageProject = manageProject;
            this.logger = logger;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                logger.LogInformation("Getting all projects");

                return Ok(await manageProject.GetAllProjectsAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }

        // GET: api/Projects/5       
        [HttpGet("{id}", Name = "GetProject")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                logger.LogInformation($"Getting project details for {id}");

                return Ok(await manageProject.GetProjectAsync(id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }

        // POST: api/Projects
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Project project)
        {
            try
            {
                if (project == null)
                {
                    logger.LogInformation($"Project is null.  Provide valid project details.");
                    return BadRequest();
                }

                await manageProject.AddProjectAsync(project);

                logger.LogInformation($"Project has been added Successfully and the new project id is { project.ProjectId }");

                return Ok(project.ProjectId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]Project project)
        {
            try
            {
                logger.LogInformation($"Updating project {id}");
                if (project == null || id != project.ProjectId)
                {
                    logger.LogInformation("Invalid project to edit");
                    return BadRequest("Invalid project to edit.");
                }

                if (!project.ActiveStatus && !manageProject.IsProjectValidToClose(project))
                {
                    logger.LogInformation("You can not close this project as the project has association with task");
                    return BadRequest("You can not close this project as the project has association with task");
                }

                await manageProject.EditProjectAsync(id, project);
                logger.LogInformation($"Project has been updated successfully for the user id { project.ProjectId } ");
                return Ok(project.ProjectId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var project = await manageProject.GetProjectAsync(id);
                if (!manageProject.IsProjectValidToClose(project))
                {
                    logger.LogInformation("You can not delete as the user have association with Task");
                    return BadRequest("You can not delete as the user have association with Task");
                }

                await manageProject.DeleteProjectAsync(project);

                return Ok(project.ProjectId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }
    }
}
