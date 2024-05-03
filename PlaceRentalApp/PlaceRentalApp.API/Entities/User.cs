namespace PlaceRentalApp.API.Entities
{
    public class User : BaseEntity
    {
        protected User() { }

        public User(string fullName, string email, DateTime birtDate)
            : base()
        {
            FullName = fullName;
            Email = email;
            BirtDate = birtDate;

            Books = [];
            Places = [];
        }

        public string FullName { get; private set; }
        public string Email { get; private set; }
        public DateTime BirtDate { get; private set; }

        public List<PlaceBook> Books { get; private set; }
        public List<Place> Places { get; private set; }
    }
}
