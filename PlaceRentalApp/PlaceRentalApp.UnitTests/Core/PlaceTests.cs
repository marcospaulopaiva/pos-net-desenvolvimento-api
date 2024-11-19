using PlaceRentalApp.Core.ValueObjects;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.UnitTests.Core
{
    public class PlaceTests
    {
        [Fact]
        public void Update_PlaceAndDateAreOk_Success()
        {
            // Arrange
            var place = new Place(
                "Title",
                "Test Description",
                100m,
                new Address("Street", "123", "12345678","District","City", "State","BR"),
                4,
                true,
                123);

            var newTitle = "New Title";
            var newDescription = "New Description";
            var newDailyPrice = 140m;

            //Act
            var result = place.Update(newTitle, newDescription, newDailyPrice);

            //Assert
            Assert.True(result);
            Assert.Equal(newTitle, place.Title);
            Assert.Equal(newDescription, place.Description);
            Assert.Equal(newDailyPrice, place.DailyPrice);

        }

        [Fact]
        public void Update_PlaceIsBlocked_False()
        {
            // Arrange
            var place = new Place(
                "Title",
                "Test Description",
                100m,
                new Address("Street", "123", "12345678", "District", "City", "State", "BR"),
                4,
                true,
                123);

            var newTitle = "New Title";
            var newDescription = "New Description";
            var newDailyPrice = 140m;

            place.Block();

            //Act
            var result = place.Update(newTitle, newDescription, newDailyPrice);

            //Assert
            Assert.False(result);
            Assert.NotEqual(newTitle, place.Title);
            Assert.NotEqual(newDescription, place.Description);
            Assert.NotEqual(newDailyPrice, place.DailyPrice);
        }

        [Fact]
        public void IsBookAllowed_AmountOfPersonAndPetOk_Success()
        {
            // Arrange
            var place = new Place(
                "Title",
                "Test Description",
                100m,
                new Address("Street", "123", "12345678", "District", "City", "State", "BR"),
                4,
                true,
                123);

            // Act
            var isBookAllowed = place.IsBookAllowed(true, 2);

            // Assert
            Assert.True(isBookAllowed);
        }

        [Fact]
        public void IsBookAllowed_HasPetButNotAllowed_False()
        {
            // Arrange
            var place = new Place(
               "Title",
               "Test Description",
               100m,
               new Address("Street", "123", "12345678", "District", "City", "State", "BR"),
               4,
               false,
               123);

            // Act
            var isBookAllowed = place.IsBookAllowed(true, 3);

            // Assert
            Assert.False(isBookAllowed);
        }

        [Fact]
        public void IsBookAllowed_OverMaximumAllowedPerson_False()
        {
            // Arrange
            var place = new Place(
               "Title",
               "Test Description",
               100m,
               new Address("Street", "123", "12345678", "District", "City", "State", "BR"),
               4,
               true,
               123);

            // Act
            var isBookAllowed = place.IsBookAllowed(true, 5);

            // Assert
            Assert.False(isBookAllowed);
        }

        public static IEnumerable<object[]> GetIsBookAllowedParams()
        {
            yield return [4, true, 2, true, true];
            yield return [4, false, 3, true, false];
            yield return [4, true, 5, true, false];
            yield return [2, true, 2, false, true];
        }

        [Theory]
        [MemberData(nameof(GetIsBookAllowedParams))]
        public void IsBookAllowed(
            int allowedNumberOfPerson,
            bool acceptPets,
            int amountOfPerson,
            bool hasPet,
            bool result)
        {
            // Arrange
            var place = new Place(
               "Title",
               "Test Description",
               100m,
               new Address("Street", "123", "12345678", "District", "City", "State", "BR"),
               allowedNumberOfPerson,
               acceptPets,
               123);

            // Act
            var isBookAllowed = place.IsBookAllowed(hasPet, amountOfPerson);

            // Assert
            Assert.Equal(result, isBookAllowed);
        }

    
    }
}
