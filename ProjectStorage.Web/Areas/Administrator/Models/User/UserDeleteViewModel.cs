namespace ProjectStorage.Web.Areas.Administrator.Models.User
{
    using Data.Models;
    using Infrastructure.Configuration;

    public class UserDeleteViewModel : IMappableFrom<User>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}