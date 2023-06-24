using System.ComponentModel.DataAnnotations;

namespace BlueBerry_API.Model.Dto
{
    public class MenuItemCreateDTO
    {
        
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpecialTag { get; set; }
        public string Category { get; set; }
        [Range(1, Int32.MaxValue)]
        public double Price { get; set; }
        
        public IFormFile File { get; set; }
    }
}
