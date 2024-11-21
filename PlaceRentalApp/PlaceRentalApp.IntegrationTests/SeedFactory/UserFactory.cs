using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.IntegrationTests.SeedFactory
{
    public class UserFactory
    {
        public static User GetUser()
            => new(
                "Full Name",
                "mail@mail.com",
                DateTime.Now.AddYears(-30),
                "12345678",
                "admin");
    }
}
