namespace ProjectStorage.Web.Areas.Project.Controllers
{
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    public class FoldersController : ProjectBaseController
    {
        private readonly IFolderService folderService;

        public FoldersController(IFolderService folderService)
        {
            this.folderService = folderService;
        }

        public IActionResult Details(string id)
        {
            return this.ViewOrNotFound(this.folderService.GetFolder(id));
        }

        public IActionResult Download(string id)
        {
            return this.File(this.folderService.ZipFolder(id), "application/zip", this.folderService.GetFolder(id).FolderName + ".zip");
        }
    }
}