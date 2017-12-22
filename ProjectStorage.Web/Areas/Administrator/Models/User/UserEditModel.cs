namespace ProjectStorage.Web.Areas.Administrator.Models.User
{
    using Data.Models;
    using Infrastructure.Configuration;
    using System.Collections.Generic;

    public class UserEditModel : IMappableFrom<User>
    {
        public string Username { get; set; }

        public IEnumerable<string> AllRoles { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}