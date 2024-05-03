using PlaceRentalApp.API.Entities;

namespace PlaceRentalApp.API.Persistence
{
    public class PlaceRentalDbContext
    {
        public PlaceRentalDbContext()
        {
            Places = [];
            PlaceAmenities = [];
            PlaceBooks = [];
            Users = [];
        }
        public List<Place> Places { get; set; }
        public List<PlaceAmenity> PlaceAmenities { get; set; }
        public List<PlaceBook> PlaceBooks { get; set; }
        public List<User> Users { get; set; }

    }
}
