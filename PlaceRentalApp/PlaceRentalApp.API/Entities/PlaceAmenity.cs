namespace PlaceRentalApp.API.Entities
{
    public class PlaceAmenity : BaseEntity
    {
        public PlaceAmenity(string description, int idPlace)
            : base()
        {
            Description = description;
            IdPlace = idPlace;
        }

        public string Description { get; private set; }
        public int IdPlace { get; private set; }
    }
}
