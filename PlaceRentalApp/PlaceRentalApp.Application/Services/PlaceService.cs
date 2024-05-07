using Microsoft.EntityFrameworkCore;
using PlaceRentalApp.Application.Exceptions;
using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Core.ValueObjects;
using PlaceRentalApp.Infrastructure.Persistence;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.Application.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly PlaceRentalDbContext _context;
        public PlaceService(PlaceRentalDbContext context)
        {
            _context = context;
        }

        public ResultViewModel Book(int id, CreateBookInputModel model)
        {
            var exists = _context.Places.Any(p => p.Id == id);

            if (!exists)
            {
                return ResultViewModel.Error("Not found");
            }

            var book = new PlaceBook(model.IdUser, model.IdPlace, model.StartDate, model.EndDate, model.Comments);


            _context.PlaceBooks.Add(book);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }

        public ResultViewModel Delete(int id)
        {
            var place = _context.Places.SingleOrDefault(p => p.Id == id);

            if (place is null)
            {
                return ResultViewModel.Error("Not found");
            }

            place.SetAsDeleted();

            _context.Places.Update(place);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }

        public ResultViewModel<List<PlaceViewModel>> GetAllAvailable(string search, DateTime startDate, DateTime endDate)
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

            var model = availablePlaces.Select(
                PlaceViewModel.FromEntity).ToList();

            return ResultViewModel<List<PlaceViewModel>>.Success(model);
        }

        public ResultViewModel<PlaceDetailsViewModel?> GetById(int id)
        {
            var place = _context.Places
                .Include(p => p.Amenities)
                .Include (p => p.User)
                .SingleOrDefault(p => p.Id == id);

            return place is null ?
                ResultViewModel<PlaceDetailsViewModel?>.Error("Not found") :
                ResultViewModel<PlaceDetailsViewModel?>.Success(PlaceDetailsViewModel.FromEntity(place));
        }

        public ResultViewModel<int> Insert(CreatePlaceInputModel model)
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

            return ResultViewModel<int>.Success(place.Id);
        }

        public ResultViewModel InsertAmenity(int id, CreatePlaceAmenityInputModel model)
        {
            var exists = _context.Places.Any(p => p.Id == id);

            if (!exists)
            {
                return ResultViewModel.Error("Not found");
            }

            var amenity = new PlaceAmenity(model.Description, id);

            _context.PlaceAmenities.Add(amenity);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }

        public ResultViewModel Update(int id, UpdatePlaceInputModel model)
        {
            var place = _context.Places.SingleOrDefault(p => p.Id == id);

            if (place is null)
            {
                return ResultViewModel.Error("Not found");
            }

            place.Update(model.Title, model.Description, model.DailyPrice);

            _context.Places.Update(place);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }
    }
}
