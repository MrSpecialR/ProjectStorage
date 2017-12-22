namespace ProjectStorage.Web.Areas.Project.Models
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class ProjectCreateModel
    {
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Title { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}