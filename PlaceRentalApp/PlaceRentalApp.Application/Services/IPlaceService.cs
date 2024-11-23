using PlaceRentalApp.Application.Models;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.Application.Services
{
    public interface IPlaceService
    {
        ResultViewModel Book(int id, CreateBookInputModel model);
        ResultViewModel Delete(int id);

        ResultViewModel<List<PlaceViewModel>> GetAllAvailable(string search, DateTime startDate, DateTime endDate);

        ResultViewModel<PlaceDetailsViewModel?> GetById(int id);

        ResultViewModel<int> Insert(CreatePlaceInputModel model);

        ResultViewModel InsertAmenity(int id, CreatePlaceAmenityInputModel model);

        ResultViewModel Update(int id, UpdatePlaceInputModel model);

        ResultViewModel Cancel(int id);
    }
}
