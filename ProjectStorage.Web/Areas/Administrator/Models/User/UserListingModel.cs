namespace ProjectStorage.Web.Areas.Administrator.Models.User
{
    using Infrastructure.Configuration;
    using System.Collections.Generic;
    using AutoMapper;
    using Data.Models;
    
    public class UserListingModel : IMappableFrom<User>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public IList<string> Roles { get; set; }
    }
}