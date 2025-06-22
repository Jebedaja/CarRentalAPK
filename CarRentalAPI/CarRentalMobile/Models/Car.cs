namespace CarRentalMobile.Models
{
    public class Car
    {
        public int Id { get; set; } 
        public string Brand { get; set; } 
        public string Model { get; set; } 
        public int Year { get; set; } 
        public decimal PricePerDay { get; set; } 
        public string ImageUrl { get; set; } 
        public bool IsAvailable { get; set; } = true;

        //public decimal DailyPrice { get; set; }

        public int CityId { get; set; }
        public City? City { get; set; } 

        
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
