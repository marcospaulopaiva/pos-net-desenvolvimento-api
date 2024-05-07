using Microsoft.AspNetCore.Mvc;
using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.Services;

namespace PlaceRentalApp.API.Controllers
{
    [Route("api/places")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceService _placeService;
        public PlacesController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        // GET api/places?search=casa&startData=2024-01-20
        [HttpGet]
        public IActionResult Get(string search, DateTime startDate, DateTime endDate)
        {
            var result = _placeService.GetAllAvailable(search, startDate, endDate);

            return Ok(result);
        }

        // GET api/places/1234
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _placeService.GetById(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST api/places
        [HttpPost]
        public IActionResult Post(CreatePlaceInputModel model)
        {
            var result = _placeService.Insert(model);

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, model);
        }

        // PUT api/places/1234
        [HttpPut]
        public IActionResult Put(int id, UpdatePlaceInputModel model) 
        {
            var result = _placeService.Update(id, model);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST api/places/1234/amenities
        [HttpPost("{id}/amenities")]
        public IActionResult PostAmenity(int id, CreatePlaceAmenityInputModel model)
        {
            var result = _placeService.InsertAmenity(id, model);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return NoContent();
        }

        //DELETE api/places/1234
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) 
        {
            var result =  _placeService.Delete(id);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST api/places/1234/books
        [HttpPost("{id}/books")]
        public IActionResult PostBook(int id, CreateBookInputModel model)
        {
            var result = _placeService.Book(id,model);
            
            if (!result.IsSuccess)
            {
                return NotFound();
            }

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
