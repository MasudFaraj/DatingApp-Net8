using API.Data; 
using API.EntitiesModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  //    <=> api/users  vorsicht: [controller] mit klammern
    public class UsersController(DataContext context) : ControllerBase // primary constructor
    {
        /*private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            // this.context = context;
            _context = context;
        }*/

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() { 
            var users = await context.Users.ToListAsync();
            return users;
        }

        [HttpGet("{id}")]  // api/users/2
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null) { return NotFound(); }
            return user;
        }
    }
}
