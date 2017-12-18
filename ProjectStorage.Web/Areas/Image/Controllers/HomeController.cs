namespace ProjectStorage.Web.Areas.Image.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : ImageBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}