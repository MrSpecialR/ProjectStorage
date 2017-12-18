namespace ProjectStorage.Web.Areas.Administrator.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data.Models;
    using Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.User;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserController : AdministratorBaseController
    {
        private readonly UserManager<User> userManager;

        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IMapper mapper;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }


        public IActionResult All()
        {
            var userRoles = this.userManager.Users.ToList().Select(async u => new
            {
                u.Id,
                Roles = (await this.userManager.GetRolesAsync(u))
            })
            .Select(u => u.Result)
            .ToList();

            return this.View(this.userManager.Users.ProjectTo<UserListingModel>().ToList().Select(u =>
            {
                u.Roles = userRoles.First(ur => ur.Id == u.Id).Roles;
                return u;
            }));
        }

        public IActionResult Delete(string id)
        {
            return this.ViewOrNotFound(this.userManager.Users.Where(u => u.Id == id).ProjectTo<UserDeleteViewModel>().FirstOrDefault());
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete_Post(string id)
        {
            User user = await this.userManager.FindByIdAsync(id);
            if (user == null)
            {
                return this.NotFound();
            }

            await this.userManager.DeleteAsync(user);

            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> Edit(string id)
        {
            User user = await this.userManager.FindByIdAsync(id);
            UserEditModel userModel = this.mapper.Map<UserEditModel>(user);
            userModel.Roles = await this.userManager.GetRolesAsync(user);
            userModel.AllRoles = this.roleManager.Roles.Select(r => r.Name).ToList();
            return this.View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserListingModel userModel)
        {
            User user = await this.userManager.FindByIdAsync(id);
            var rolesToRemove = await this.userManager.GetRolesAsync(user);
            await this.userManager.RemoveFromRolesAsync(user, rolesToRemove);
            await this.userManager.AddToRolesAsync(user, userModel.Roles);

            return this.RedirectToAction("All");
        }
    }
}