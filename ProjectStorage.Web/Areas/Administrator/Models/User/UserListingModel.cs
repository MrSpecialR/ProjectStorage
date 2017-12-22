namespace ProjectStorage.Web.Areas.Administrator.Models.User
{
    using Data.Models;
    using Infrastructure.Configuration;
    using System.Collections.Generic;

    public class UserListingModel : IMappableFrom<User>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public IList<string> Roles { get; set; }
    }
}