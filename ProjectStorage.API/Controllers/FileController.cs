namespace ProjectStorage.API.Controllers
{
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Text;

    public class FileController : BaseController
    {
        private readonly IFileService fileService;

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.OkOrNotFound(this.fileService.GetFiles());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id, bool contentOnly = false)
        {
            var file = this.fileService.GetFileById(id);
            if (contentOnly)
            {
                return this.OkOrNotFound(Encoding.Default.GetString(file.Content));
            }
            return this.OkOrNotFound(file);
        }
    }
}