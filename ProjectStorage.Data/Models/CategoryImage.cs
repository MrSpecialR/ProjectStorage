using System;

namespace ProjectStorage.Data.Models
{
    public class CategoryImage
    {
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public Guid ImageId { get; set; }

        public Image Image { get; set; }
    }
}