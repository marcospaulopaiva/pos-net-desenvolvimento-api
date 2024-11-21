using PlaceRentalApp.Core.ValueObjects;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.IntegrationTests.SeedFactory
{
    public class PlaceFactory
    {
        public static Place GetPlace(int userId)
            => new (
                "Title",
                "Test Description",
                100m,
                new Address("Street", "123", "12345678", "District", "City", "State", "BR"),
                4,
                true,
                userId);

        public static List<PlaceAmenity> GetAmenities(int placeId)
            => [
                new PlaceAmenity("2 Bedrooms", placeId),
                new PlaceAmenity("Hairdryer", placeId),
                new PlaceAmenity("Air Conditioner", placeId)
                ];
    }
}
