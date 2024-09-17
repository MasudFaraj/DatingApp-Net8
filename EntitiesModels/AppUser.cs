namespace API.EntitiesModels
{
    public class AppUser
    {
        public int Id { get; set; } // mit Id Bennenung wird als PK gesehen
        public required string Name { get; set; }

        public required byte[] PasswordHash { get; set;  }  // hashed & encrypted
        public required byte[] PasswordSalt { get; set; }
       
    }
}
