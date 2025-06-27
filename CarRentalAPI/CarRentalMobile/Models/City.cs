namespace CarRentalMobile.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }


        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}