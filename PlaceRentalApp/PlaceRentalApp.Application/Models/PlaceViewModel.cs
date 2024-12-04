using static PlaceRentalApp.Core.Entities.BaseEntity;

namespace PlaceRentalApp.Application.Models
{
    public class PlaceViewModel
    {
        public PlaceViewModel(int id, string title, string description, decimal dailyPrice, string address, string createdBy)
        {
            Id = id;
            Title = title;
            Description = description;
            DailyPrice = dailyPrice;
            Address = address;
            CreatedBy = createdBy;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal DailyPrice { get; set; }
        public string Address { get; set; }
        public string CreatedBy { get; set; }

        public static PlaceViewModel? FromEntity(Place entity)
            => new(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.DailyPrice,
                entity.Address.GetFullAddress(),
                entity.User.FullName
                );
    }
}
