﻿namespace ProjectStorage.Data.Models
{
    using System.Collections.Generic;

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<CategoryImage> Images { get; set; } = new List<CategoryImage>();
    }
}