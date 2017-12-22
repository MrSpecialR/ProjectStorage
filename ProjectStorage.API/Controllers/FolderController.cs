namespace ProjectStorage.API.Controllers
{
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services;


    public class FolderController : BaseController
    {
        private readonly IFolderService folderService;

        public FolderController(IFolderService folderService)
        {
            this.folderService = folderService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.OkOrNotFound(this.folderService.GetFolders());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return this.OkOrNotFound(this.folderService.GetFolder(id));
        }
    }
}