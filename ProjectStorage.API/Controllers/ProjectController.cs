namespace ProjectStorage.API.Controllers
{
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    public class ProjectController : BaseController
    {
        private readonly IProjectService projectService;

        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.OkOrNotFound(this.projectService.GetAllProjects());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.OkOrNotFound(this.projectService.GetProjectServiceModel(id));
        }
    }
}