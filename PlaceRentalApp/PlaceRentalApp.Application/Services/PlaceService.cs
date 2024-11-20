using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Core.Repositories;
using PlaceRentalApp.Core.ValueObjects;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.Application.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceRepository _placeRepository;

        public PlaceService(IPlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        public ResultViewModel Book(int id, CreateBookInputModel model)
        {
            var place = _placeRepository.GetById(id);

            if (place is null)
            {
                return ResultViewModel.Error("Not found");
            }

            var book = new PlaceBook(model.IdUser, model.IdPlace, model.StartDate, model.EndDate, model.Comments);

            _placeRepository.AddBook(book); 

            return ResultViewModel.Success();
        }

        public ResultViewModel Delete(int id)
        {
            var place = _placeRepository.GetById(id);

            if (place is null)
            {
                return ResultViewModel.Error("Not found");
            }

            place.SetAsDeleted();

            _placeRepository.Delete(place);

            return ResultViewModel.Success();
        }

        public ResultViewModel<List<PlaceViewModel>> GetAllAvailable(string search, DateTime startDate, DateTime endDate)
        {
            var availablePlaces = 
                _placeRepository.GetAllAvailable(search, startDate, endDate) ?? [];

            var model = availablePlaces.Select(
                PlaceViewModel.FromEntity).ToList();

            return ResultViewModel<List<PlaceViewModel>>.Success(model);
        }

        public ResultViewModel<PlaceDetailsViewModel?> GetById(int id)
        {
            var place = _placeRepository.GetById(id);

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

            var id = _placeRepository.Add(place);

            return ResultViewModel<int>.Success(id);
        }

        public ResultViewModel InsertAmenity(int id, CreatePlaceAmenityInputModel model)
        {
            var place = _placeRepository.GetById(id);

            if (place is null)
            {
                return ResultViewModel.Error("Not found");
            }

            var amenity = new PlaceAmenity(model.Description, id);

            _placeRepository.AddAmenity(amenity);

            return ResultViewModel.Success();
        }

        public ResultViewModel Update(int id, UpdatePlaceInputModel model)
        {
            var place = _placeRepository.GetById(id);

            if (place is null)
            {
                return ResultViewModel.Error("Not found");
            }

            place.Update(model.Title, model.Description, model.DailyPrice);

            _placeRepository.Update(place);

            return ResultViewModel.Success();
        }
    }
}
