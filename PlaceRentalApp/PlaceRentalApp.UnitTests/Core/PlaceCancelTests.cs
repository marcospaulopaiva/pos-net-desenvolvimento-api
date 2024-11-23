using Bogus;
using FluentAssertions;
using PlaceRentalApp.Core.Enums;
using PlaceRentalApp.UnitTests.Fakes;
using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.UnitTests.Core
{
    public class PlaceCancelTests
    {
        [Fact]
        public void HasNoActiveBooks_Success()
        {
            // Arrange
            var place = new PlaceFake()
                .Generate();

            // Act
            place.Cancel();

            // Assert
            place.Status.Should().Be(PlaceStatus.Inactive);
        }

        [Fact]
        public void HasNoActiveBooks_Error() 
        {
            // Arrange
            var place = new PlaceFake()
               .RuleFor(
                    p => p.Books, new Faker<PlaceBook>()
                         .RuleFor(pb => pb.EndDate, _ => DateTime.Now.AddDays(4))
                         .RuleFor(pb => pb.StartDate, _ => DateTime.Now.AddDays(-1))
                         .Generate(1))
               .Generate();

            // Act 
            var action = () => place.Cancel();

            // Assert
            action.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("Invalid status");
        }

    }
}
