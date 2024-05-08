using Microsoft.EntityFrameworkCore;
using PlaceRentalApp.Core.Repositories;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.Infrastructure.Persistence.Repositories
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly PlaceRentalDbContext _context;

        public PlaceRepository(PlaceRentalDbContext context)
        {
            _context = context;
        }

        public int Add(Place place)
        {
            _context.Places.Add(place);
            _context.SaveChanges();

            return place.Id;
        }

        public void AddAmenity(PlaceAmenity amenity)
        {
            _context.PlaceAmenities.Add(amenity);
            _context.SaveChanges();
        }

        public void AddBook(PlaceBook book)
        {
            _context.PlaceBooks.Add(book);
            _context.SaveChanges();
        }

        public void Delete(Place place)
        {
            _context.Places.Update(place);
            _context.SaveChanges();
        }

        public List<Place>? GetAllAvailable(string search, DateTime startDate, DateTime endDate)
        {
            var availablePlaces = _context
                .Places
                .Include(p => p.User)
                .Where(p =>
                    p.Title.Contains(search) &&
                    !p.Books.Any(b =>
                    (startDate >= b.StartDate && startDate <= b.EndDate) ||
                    (endDate >= b.StartDate && endDate <= b.EndDate) ||
                    (startDate <= b.StartDate && endDate >= b.EndDate))
                    && !p.IsDeleted)
                .ToList();

            return availablePlaces;
        }

        public Place? GetById(int id)
        {
            var place = _context.Places
                .Include(p => p.Amenities)
                .Include(p => p.User)
                .SingleOrDefault(p => p.Id == id);

            return place;
        }

        public void Update(Place place)
        {
            _context.Places.Update(place);
            _context.SaveChanges();
        }
    }
}
