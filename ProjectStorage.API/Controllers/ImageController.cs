namespace ProjectStorage.API.Controllers
{
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    public class ImageController : BaseController
    {
        private readonly IImageService imageService;

        public ImageController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.OkOrNotFound(this.imageService.GetAllImagesManage());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return this.OkOrNotFound(this.imageService.GetImageById(id));
        }
    }
}