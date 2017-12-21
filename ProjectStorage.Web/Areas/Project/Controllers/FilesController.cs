namespace ProjectStorage.Web.Areas.Project.Controllers
{
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Collections.Generic;
    using System.Linq;  

    public class FilesController : ProjectBaseController
    {
        private readonly IList<string> textFileFormats;
        private readonly IFileService fileService;

        public FilesController(IFileService fileService)
        {
            this.fileService = fileService;
            this.textFileFormats = new List<string>
            {
                "txt",
                "cs",
                "js",
                "cpp",
                "rtf",
                "csproj",
                "sln"
            };
        }

        public IActionResult Details(string id)
        {
            var file = this.fileService.GetFileById(id);
            if (file == null)
            {
                return this.NotFound();
            }
            var extension = file.Path.Split('.').LastOrDefault();
            if (this.textFileFormats.Contains(extension))
            {
         
                return this.View("DetailsText", file);
            }

            return this.ViewOrNotFound(file);
        }

        public IActionResult Delete(string id)
        {
            return this.ViewOrNotFound(this.fileService.GetFileById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult Delete_Post(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return this.NotFound();
            }

            this.fileService.Delete(id);

            return this.RedirectToAction("Index", "Manage", new { area = "Project" });
        }

        public IActionResult Download(string id)
        {
            var file = this.fileService.GetFileById(id);
            return this.File(file.Content, "application/octet-stream", file.Name);
        }
    }
}