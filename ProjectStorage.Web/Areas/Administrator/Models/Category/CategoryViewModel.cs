namespace ProjectStorage.Web.Areas.Administrator.Models.Category
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryViewModel
    {
        [StringLength(100, MinimumLength = 5)]
        [Required]
        public string Name { get; set; }
    }
}