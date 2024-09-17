using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        //[Required]   // data annotation
        //[MaxLength(100)]    kann für Validation benutzt werden
        public required string Name { get; set; }
        
        //[Required]
        public required string Password { get; set; }
    }
}
