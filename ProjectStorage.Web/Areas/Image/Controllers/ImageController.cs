namespace ProjectStorage.Web.Areas.Image.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ImageController : ImageBaseController
    {
        // GET
        public IActionResult Create()
        {
            return View();
        }
    }
}