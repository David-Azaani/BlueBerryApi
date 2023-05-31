using System.ComponentModel.DataAnnotations;

namespace BlueBerry_API.Model
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }    
        public string Description { get; set; }
        public string SpecialTag { get; set; }        
        public string Category { get; set; }
        [Range(1,Int32.MaxValue)]
        public double Price { get; set; }
        [Required]
        public string Image { get; set; }        

    }
}
