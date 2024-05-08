using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Core.Repositories;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ResultViewModel<User?> GetById(int id)
        {
            var user = _userRepository.GetById(id);

            return user is null ?
                ResultViewModel<User?>.Error("Not found") :
                ResultViewModel<User?>.Success(user);
        }

        public ResultViewModel<int> Insert(CreateUserInputModel model)
        {
            var user = new User(model.FullName, model.Email, model.BirthDate);

            _userRepository.Add(user);

            return ResultViewModel<int>.Success(user.Id);
        }
    }
}
