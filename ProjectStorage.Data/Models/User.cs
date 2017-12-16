namespace ProjectStorage.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public IList<Project> Projects { get; set; } = new List<Project>();
    }
}
