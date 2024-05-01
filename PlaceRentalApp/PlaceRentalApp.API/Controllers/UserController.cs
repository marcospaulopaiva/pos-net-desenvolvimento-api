using Microsoft.AspNetCore.Mvc;
using PlaceRentalApp.API.Models;

namespace PlaceRentalApp.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post(CreateUserInputModel model)
        {
            return CreatedAtAction(nameof(GetById), new {id = 1}, model);
        }
    }
}
