using PlaceRentalApp.Application.Models;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.Application.Services
{
    public interface IUserService
    {
        ResultViewModel<User?> GetById(int id);
        ResultViewModel<int> Insert(CreateUserInputModel model);
        ResultViewModel<LoginViewModel?> Login(LoginInputModel model);
    }
}
