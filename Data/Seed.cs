using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.EntitiesModels;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)   // Task without type Parameter dont retur anything
        {
            if (await context.Users.AnyAsync())  return;  // if we continue --> we dont have any user in our db

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            foreach(var user in users){
                using var hmac = new HMACSHA512();
                user.Name = user.Name.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;
                context.Users.Add(user);
            }
            await context.SaveChangesAsync(); 
        }
    }
}
