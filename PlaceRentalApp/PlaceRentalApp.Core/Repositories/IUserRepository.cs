using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.Core.Repositories
{
    public interface IUserRepository
    {
        int Add(User user);

        User? GetById(int id);

        User? GetByLoginAndHash(string email, string hash);
    }
}
