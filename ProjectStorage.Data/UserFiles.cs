using System;
using ProjectStorage.Data.Models;

namespace ProjectStorage.Data
{
    public class UserFiles
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public Guid ImageId { get; set; }

        public Image Image { get; set; }
    }
}