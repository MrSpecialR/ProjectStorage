namespace ProjectStorage.Web.Areas.Administrator.Controllers
{
    using Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(GlobalConstants.AdministratorArea)]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public abstract class AdministratorBaseController : Controller
    {
    }
}