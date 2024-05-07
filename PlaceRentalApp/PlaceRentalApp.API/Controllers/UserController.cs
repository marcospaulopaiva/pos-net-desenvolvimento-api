using Microsoft.AspNetCore.Mvc;
using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.Services;

namespace PlaceRentalApp.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(CreateUserInputModel model)
        {
            var id = _userService.Insert(model);

            return CreatedAtAction(nameof(GetById), new { id }, model);
        }
    }
}
