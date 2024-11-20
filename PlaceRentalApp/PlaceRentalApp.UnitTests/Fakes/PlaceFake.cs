using Bogus;
using PlaceRentalApp.Core.ValueObjects;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.UnitTests.Fakes
{
    public class AddressFake : Faker<Address>
    {
        public AddressFake() 
        {
            CustomInstantiator(faker =>
                new Address(
                    faker.Address.StreetName(),
                    faker.Address.BuildingNumber(),
                    faker.Address.ZipCode(),
                    faker.Address.Country(),
                    faker.Address.City(),
                    faker.Address.State(),
                    faker.Address.Country()
                )
            );        
        }
    }

    public class UserFake: Faker<User>
    {
        public UserFake()
        {
            CustomInstantiator(faker =>
                new User(
                    faker.Name.FullName(),
                    faker.Internet.Email(),
                    faker.Date.Recent(365),
                    faker.Internet.Password(),
                    "admim"
                )
            );
        }
    }

    public class AmenityFake: Faker<PlaceAmenity>
    {
        public AmenityFake()
        {
            CustomInstantiator(faker =>
            new PlaceAmenity(
                faker.Random.Word(),
                faker.Random.Number(1, 800))
            );
        }
    }

    public class PlaceFake: Faker<Place> 
    {
        public PlaceFake()
        {
            CustomInstantiator(faker =>
            new Place(
                faker.Random.Word(),
                faker.Random.Word(),
                faker.Random.Number(1, 200),
                new AddressFake().Generate(),
                faker.Random.Number(1, 5),
                true,
                faker.Random.Number(1, 800)
                ));

            RuleFor(p => p.User, f => new UserFake().Generate());
            RuleFor(p => p.Amenities, f => new AmenityFake().Generate(5));
        }
    }
}
