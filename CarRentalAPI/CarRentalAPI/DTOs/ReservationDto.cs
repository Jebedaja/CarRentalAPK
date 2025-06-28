using System;
using CarRentalAPI.Models; // Nadal używasz modeli dla mapowania

namespace CarRentalAPI.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int RentalDays { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CarId { get; set; }
        public CarDto? Car { get; set; } // Używamy DTO dla samochodu
    }
}