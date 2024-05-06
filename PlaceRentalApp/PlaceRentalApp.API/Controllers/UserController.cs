using Microsoft.AspNetCore.Mvc;
using PlaceRentalApp.API.Models;
using PlaceRentalApp.API.Persistence;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PlaceRentalDbContext _context;
        public UserController(PlaceRentalDbContext context)
        {
            this._context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(CreateUserInputModel model)
        {
            var user = new User(model.FullName, model.Email, model.BirthDate);

            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { user.Id }, model);
        }
    }
}
