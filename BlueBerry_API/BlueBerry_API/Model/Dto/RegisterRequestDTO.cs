using System.ComponentModel.DataAnnotations;

namespace BlueBerry_API.Model.Dto
{
    public class RegisterRequestDTO
    {
        [Required]
        public string  Username { get; set; }
        public string  Name { get; set; }
        [Required]
        public string  Password { get; set; }
        public string  Role { get; set; }
    }
}
