using PlaceRentalApp.Core.Repositories;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.Infrastructure.Persistence.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly PlaceRentalDbContext _context;

        public UserRepository(PlaceRentalDbContext context)
        {
            _context = context;
        }

        public int Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user.Id;
        }

        public User? GetById(int id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);

            return user;
        }

        public User? GetByLoginAndHash(string email, string hash)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email && u.Password == hash);

            return user;
        }
    }
}
