namespace ProjectStorage.Web.Areas.Project.Controllers
{
    using Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(GlobalConstants.ProjectArea)]
    [Authorize]
    public abstract class ProjectBaseController : Controller
    {
    }
}