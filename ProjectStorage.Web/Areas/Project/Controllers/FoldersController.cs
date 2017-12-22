namespace ProjectStorage.Web.Areas.Project.Controllers
{
    using Constants;
    using Data.Models;
    using Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System;

    public class FoldersController : ProjectBaseController
    {
        private readonly IFolderService folderService;
        private readonly UserManager<User> userManager;

        public FoldersController(IFolderService folderService, UserManager<User> userManager)
        {
            this.folderService = folderService;
            this.userManager = userManager;
        }

        public IActionResult Details(string id)
        {
            return this.ViewOrNotFound(this.folderService.GetFolder(id));
        }

        public IActionResult Download(string id)
        {
            return this.File(this.folderService.ZipFolder(id), "application/zip", this.folderService.GetFolder(id).FolderName + ".zip");
        }

        public IActionResult Delete(string id)
        {
            return this.ViewOrNotFound(this.folderService.GetFolder(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult Delete_Post(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return this.NotFound();
            }
            if (!this.User.IsInRole(GlobalConstants.ProjectTesterRole) &&
                !this.folderService.IsOwner(this.userManager.GetUserId(User), id))
            {
                return this.Redirect("/Account/Login");
            }

            throw new NotImplementedException();
            this.folderService.Delete(id);

            return this.RedirectToAction("Browse", "Manage", new { area = "Project" });
        }
    }
}