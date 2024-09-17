using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   //    <=> api/users  vorsicht: [controller] mit klammern
    public class BaseApiController : ControllerBase
    {

    }
}
