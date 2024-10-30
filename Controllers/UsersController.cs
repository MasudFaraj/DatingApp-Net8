using API.Data; 
using API.EntitiesModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace API.Controllers
{   
    public class UsersController(DataContext context) : BaseApiController // primary constructor
    {
        /*private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            // this.context = context;
            _context = context;
        }*/
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() {   // List<AppUser>
            var users = await context.Users.ToListAsync();     // hier kein service, Repo, Query, Command
            return users;  // hier wir sind wie in Repo
        }

        [AllowAnonymous]
        //[Authorize] //i dont get the user , despite the correct configurations ?!  it costed me too many time
        [HttpGet("{id}")]  // api/users/2
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null) { return NotFound(); }
            return user;
        }

        [HttpDelete("{id}")]  // api/users/{id}
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            context.Users.Remove(user);  /////
            await context.SaveChangesAsync();

            return Ok($"User with ID {id} has been deleted.");
        }

    }
}
