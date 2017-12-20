namespace ProjectStorage.Web.Areas.Project.Models
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class ProjectCreateModel
    {
        [Required]
        public IFormFile File { get; set; }
    }
}