namespace ProjectStorage.Web.Areas.Project.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    public class ManageController : ProjectBaseController
    {
        private readonly IProjectService projectService;
        private readonly UserManager<User> userManager;

        public ManageController(IProjectService projectService, UserManager<User> userManager)
        {
            this.projectService = projectService;
            this.userManager = userManager;
        }


        public IActionResult Upload()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Upload(ProjectCreateModel file)
        {
            this.projectService.Add(this.userManager.GetUserId(this.User), file.File);

            return this.RedirectToAction("Upload");
        }
    }
}