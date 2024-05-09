using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Core.Repositories;
using PlaceRentalApp.Infrastructure.Auth;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
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
            var hash = _authService.ComputeHash(model.Password);

            var user = new User(model.FullName, model.Email, model.BirthDate, hash, model.Role);

            _userRepository.Add(user);

            return ResultViewModel<int>.Success(user.Id);
        }

        public ResultViewModel<LoginViewModel?> Login(LoginInputModel model)
        {
            var hash = _authService.ComputeHash(model.Password);

            var user = _userRepository.GetByLoginAndHash(model.Email, hash);

            if (user is null)
            {
                return ResultViewModel<LoginViewModel?>.Error("Error");
            }

            var token = _authService.GenerateToken(user.Email, user.Role);

            var viewModel = new LoginViewModel(token);

            return ResultViewModel<LoginViewModel?>.Success(viewModel);
        }
    }
}
