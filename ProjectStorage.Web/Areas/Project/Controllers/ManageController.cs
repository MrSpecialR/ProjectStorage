namespace ProjectStorage.Web.Areas.Project.Controllers
{
    using Constants;
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
            if (!this.ModelState.IsValid)
            {
                return this.View(file);
            }
            var id = this.projectService.Add(this.userManager.GetUserId(this.User), file.File, file.Title);

            return this.RedirectToAction("Details", new { id });
        }

        public IActionResult Browse()
        {
            return this.View(this.projectService.GetAllProjects());
        }

        public IActionResult Details(int id)
        {
            return this.View(this.projectService.GetProject(id));
        }

        public IActionResult Delete(int id)
        {
            if (!this.User.IsInRole(GlobalConstants.ProjectTesterRole) && !this.projectService.UserIsOwner(id, this.userManager.GetUserId(this.User)))
            {
                return this.Redirect("/Users/Login");
            }

            return this.View(this.projectService.GetProject(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult Delete_Post(int id)
        {
            if (!this.User.IsInRole(GlobalConstants.ProjectTesterRole) && !this.projectService.UserIsOwner(id, this.userManager.GetUserId(this.User)))
            {
                return this.Redirect("/Users/Login");
            }

            this.projectService.Delete(id);
            return this.RedirectToAction("Browse", "Manage");
        }

        public IActionResult Download(int id)
        {
            var project = this.projectService.GetProject(id);
            if (project == null)
            {
                return this.NotFound();
            }
            return this.File(this.projectService.ZipProject(id), "application/zip", project.FolderName + ".zip");
        }
    }
}