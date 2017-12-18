namespace ProjectStorage.Web.Extensions
{
    using Microsoft.AspNetCore.Mvc;

    public static class ControllerExtension
    {
        public static IActionResult ViewOrNotFound(this Controller controller, object model)
        {
            if (model == null)
            {
                return controller.NotFound();
            }

            return controller.View(model);
        }
    }
}
