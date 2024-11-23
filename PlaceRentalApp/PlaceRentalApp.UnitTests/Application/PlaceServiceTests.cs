using NSubstitute;
using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.Services;
using PlaceRentalApp.Core.Repositories;
using PlaceRentalApp.UnitTests.Fakes;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.UnitTests.Application
{
    public class PlaceServiceTests
    {
        [Fact]
        public void Insert_DataIsOk_Success()
        {
            // Arrange
            var createPlaceInputModel = new CreatePlaceInputModel()
            {
                Title = "Charming Beach House",
                Description = "A beautiful and relaxing beach house located",
                DailyPrice = 150.00m,
                Address = new AddressInputModel
                {
                    Street = "123 Ocean View",
                    Number = "100",
                    District = "Seafront",
                    City = "Seaside",
                    State = "CA",
                    Country = "USA"
                },
                AllowedNumberPerson = 4,
                AllowPets = true,
                CreatedBy = 1
            };

            var repository = Substitute.For<IPlaceRepository>();

            repository
                .Add(Arg.Any<Place>())
                .Returns(1);

            var service = new PlaceService(repository);

            // Act
            var result = service.Insert(createPlaceInputModel);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Data);

            repository.ReceivedWithAnyArgs().Add(default);

            repository.Received(1).Add(Arg.Is<Place>(
                p => p.Title == createPlaceInputModel.Title &&
                p.DailyPrice == createPlaceInputModel.DailyPrice));

        }

        [Fact]
        public void GetById_Exists_Success()
        {
            // Arrange
            var place = new PlaceFake().Generate();
            var repository = Substitute.For<IPlaceRepository>();

            repository
                .GetById(1)
                .Returns(place);

            // Act
            var result = new PlaceService(repository).GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.True(result.IsSuccess);
            Assert.Equal(place.Id, result.Data.Id);

            repository.Received(1).GetById(1);
        }

        [Fact]
        public void Cancel_PlaceIsOk_Success()
        {
            // Arrange
            var id = 123;
            var place = new PlaceFake(id)
                .Generate();

            var repository = Substitute.For<IPlaceRepository>();

            repository
                .GetById(Arg.Any<int>())
                .Returns(place);

            var service = new PlaceService(repository);

            // Act
            var result = service.Cancel(id);


            // Assert
            Assert.True(result.IsSuccess);

            repository
                .Received(1)
                .GetById(id);
        }
    }
}
