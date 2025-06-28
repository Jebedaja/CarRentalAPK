namespace CarRentalAPI.DTOs
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        // UWAGA: Brak właściwości ICollection<Reservation> Reservations, aby uniknąć cyklicznych referencji
        // Brak właściwości City City, ponieważ nie potrzebujemy jej w ReservationDto
    }
}