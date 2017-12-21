namespace ProjectStorage.Web.Areas.Image.Models.Image
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ImageEditViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Title { get; set; }

        [Required]
        public IEnumerable<int> Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}