using API.Extentions;
using System.Diagnostics.CodeAnalysis;
using System.Printing;

namespace API.EntitiesModels
{
    public class AppUser
    {
        public int Id { get; set; } // mit Id Bennenung wird als PK gesehen
        public required string Name { get; set; }

        public  byte[] PasswordHash { get; set;  }  // hashed & encrypted
        public  byte[] PasswordSalt { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime LastActive { get; set; } = DateTime.Now;
        public string KnownAs { get; set; } = string.Empty; 

        public string Gender { get; set; } = string.Empty;
        public string Introduction { get; set; } = string.Empty ;
        public string LookingFor { get; set; } = string.Empty;
        public string Interests { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public ICollection<Photo> Photos { get; set; }  // one to many (one user can have many Photos)

        public int getAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}
