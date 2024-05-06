using Microsoft.AspNetCore.Mvc;
using PlaceRentalApp.API.Models;
using PlaceRentalApp.API.Persistence;
using PlaceRentalApp.Core.ValueObjects;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.API.Controllers
{
    [Route("api/places")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly PlaceRentalDbContext _context;
        public PlacesController(PlaceRentalDbContext context)
        {
            this._context = context;
        }

        // GET api/places?search=casa&startData=2024-01-20
        [HttpGet]
        public IActionResult Get(string search, DateTime startDate, DateTime endDate)
        {
            var availablePlaces = _context
                .Places
                .Where(p =>
                    p.Title.Contains(search) &&
                    !p.Books.Any(b =>
                    (startDate >= b.StartDate && startDate <= b.EndDate) ||
                    (endDate >= b.StartDate && endDate <= b.EndDate) ||
                    (startDate <= b.StartDate && endDate >= b.EndDate))
                    && !p.IsDeleted);

            return Ok(availablePlaces);
        }

        // GET api/places/1234
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var place = _context.Places.SingleOrDefault(p => p.Id == id);
            
            if (place is null) 
            {
                return NotFound();
            }

            return Ok(place);
        }

        // POST api/places
        [HttpPost]
        public IActionResult Post(CreatePlaceInputModel model)
        {
            var address = new Address(
                model.Address.Street,
                model.Address.Number,
                model.Address.ZipCode,
                model.Address.District,
                model.Address.City,
                model.Address.State,
                model.Address.Country
                );

            var place = new Place(
                model.Title,
                model.Description,
                model.DailyPrice,
                address,
                model.AllowedNumberPerson,
                model.AllowPets,
                model.CreatedBy
                );

            _context.Places.Add(place);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = place.Id}, model);
        }

        // PUT api/places/1234
        [HttpPut]
        public IActionResult Put(int id, UpdatePlaceInputModel model) 
        {
            var place = _context.Places.SingleOrDefault(p => p.Id == id);

            if (place is null)
            {
                return NotFound();
            }

            place.Update(model.Title, model.Description, model.DailyPrice);

            _context.Places.Update(place);
            _context.SaveChanges();

            return NoContent();
        }

        // POST api/places/1234/amenities
        [HttpPost("{id}/amenities")]
        public IActionResult PostAmenity(int id, CreatePlaceAmenityInputModel model)
        {
            var exists = _context.Places.Any(p => p.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            var amenity = new PlaceAmenity(model.Description, id);

            _context.PlaceAmenities.Add(amenity);
            _context.SaveChanges();

            return NoContent();
        }

        //DELETE api/places/1234
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) 
        {
            var place = _context.Places.SingleOrDefault(p => p.Id == id);

            if (place is null)
            {
                return NotFound();
            }

            place.SetAsDeleted();

            _context.Places.Update(place);
            _context.SaveChanges();

            return NoContent();
        }

        // POST api/places/1234/books
        [HttpPost("{id}/books")]
        public IActionResult PostBook(int id, CreateBookInputModel model)
        {
            var exists = _context.Places.Any(p => p.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            var book = new PlaceBook(model.IdUser, model.IdPlace, model.StartDate, model.EndDate, model.Comments);
 

            _context.PlaceBooks.Add(book);
            _context.SaveChanges();

            return NoContent();
        }

        // POST api/places/1234/comments
        [HttpPost("{id}/comments")]
        public IActionResult PostComment(int id, CreateCommentInputModel model) 
        {
            return NoContent();
        }

        // POST api/places/1234/photos
        [HttpPost("{id}/photos")]
        public IActionResult PostPlacePhoto(int id, IFormFile file)
        {
            var description = $"File: {file.FileName}, Size: {file.Length}";

            using var ms = new MemoryStream();
            file.CopyTo(ms);

            var fileBytes = ms.ToArray();
            var base64 = Convert.ToBase64String(fileBytes);

            return Ok(new { description, base64 });
        }
    }
}
