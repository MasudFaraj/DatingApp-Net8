using API.Data;
using API.EntitiesModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Windows.Markup;

namespace API.Controllers
{
    public class BuggyController(DataContext _context) : BaseApiController
    {
        //private readonly DataContext _context = context;
        [Authorize]
        [HttpGet("auth")]   // api/buggy/auth
        public ActionResult<string> GetSecret() {
            return "secret text";
        }
        
        [HttpGet("not-found")]   // api/buggy/not-found
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null) return NotFound();
            return thing;
        }

        [HttpGet("server-error")]   // api/buggy/server-error
        public ActionResult<string> GetServerError()
        {
           
                var thing = _context.Users.Find(-1);
                var thingToReturn = thing.ToString();
                return thingToReturn;
            /*try {
            } catch(Exception) {
                return StatusCode(500, "computer says no!");
            }*/
           
        }

        [HttpGet("bad-request")]   // api/buggy/auth
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("this was not a good Request");
        }
    }
}
