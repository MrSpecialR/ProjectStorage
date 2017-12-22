namespace ProjectStorage.API.Extensions
{
    using Microsoft.AspNetCore.Mvc;

    public static class ControllerExtension
    {
        public static IActionResult OkOrNotFound(this Controller controller, object model)
        {
            if (model == null)
            {
                return controller.NotFound();
            }

            return controller.Ok(model);
        }
    }
}