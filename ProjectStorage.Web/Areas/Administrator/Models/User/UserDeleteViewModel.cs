namespace ProjectStorage.Web.Areas.Administrator.Models.User
{
    using Infrastructure.Configuration;

    public class UserDeleteViewModel : IMappableFrom<Data.Models.User>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

    }
}