using PlaceRentalApp.Application.Models;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.Application.Services
{
    public interface IUserService
    {
        User? GetById(int id);

        int Insert(CreateUserInputModel model);
    }
}
