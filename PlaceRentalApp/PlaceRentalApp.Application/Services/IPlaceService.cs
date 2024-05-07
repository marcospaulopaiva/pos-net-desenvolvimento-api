using PlaceRentalApp.Application.Models;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.Application.Services
{
    public interface IPlaceService
    {
        void Book(int id, CreateBookInputModel model);
        void Delete(int id);

        List<PlaceViewModel> GetAllAvailable(string search, DateTime startDate, DateTime endDate);

        PlaceDetailsViewModel? GetById(int id);

        int Insert(CreatePlaceInputModel model);

        void InsertAmenity(int id, CreatePlaceAmenityInputModel model);

        void Update(int id, UpdatePlaceInputModel model);
    }
}
