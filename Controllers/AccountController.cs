using API.Data;
using API.DTOs;
using API.EntitiesModels;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")]     //  api/account/register
        //public async Task<ActionResult<IEnumerable<AppUser>>> Register(string un, string pw)
        /*Correct the Return Type: Since you're returning a single UserDto, the return type should be ActionResult<UserDto>, not ActionResult<IEnumerable<UserDto>>.
        Update the Method Signature: Change the method signature to return a single UserDto instead of a collection.*/
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Name)) {
                return BadRequest("username is taken"); 
            }

            using var hmac = new HMACSHA512();
            var user = new AppUser  ///
            {
                Name = registerDto.Name.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),  // for Encoding use system.text
                PasswordSalt = hmac.Key
            };
            context.Users.Add(user);  /////
            await context.SaveChangesAsync();

            return new UserDto {
                Name = user.Name,
                Token = tokenService.CreateToken(user)  ////
            };
        }

        [HttpPost("login")]  //wawa
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){  // to Async belongs await
            // context.Users.FindAsync(id/PK)
            var user = await context.Users.FirstOrDefaultAsync(x => x.Name.ToLower() == loginDto.Name.ToLower());
            //var user = await context.Users.SingleOrDefaultAsync(x => x.Name.ToLower() == loginDto.Name.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);    // in that way, when we compute the passwordhash, is going to have the same pw in DB
            
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i=0; i<computedHash.Length; i++ ){
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            }

            return new UserDto{
                Name=user.Name,
                Token= tokenService.CreateToken(user)
            };
        } 

        private async Task<bool> UserExists(string username)
        {
            // we want to see if any user have the same name (es wird lambda expression benutzt)
            // turn await context.Users.AnyAsync(x => x.Name.Equals(username, StringComparison.CurrentCultureIgnoreCase)); //in ef geht nicht
            return await context.Users.AnyAsync(x => x.Name.ToLower() == username.ToLower()); ///// Bob != bob
        }

    }
}
