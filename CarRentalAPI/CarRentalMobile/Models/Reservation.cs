using System;
using System.ComponentModel.DataAnnotations;

namespace CarRentalMobile.Models
{
    public class Reservation
    {
        public int Id { get; set; } 
        public decimal TotalCost { get; set; } 
        public string Status { get; set; } = "Pending"; 

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Range(21, 120, ErrorMessage = "Wiek musi być między 21 a 120 lat.")] // Walidacja wieku
        public int Age { get; set; }

        [Range(1, 365, ErrorMessage = "Ilość dni rezerwacji musi być między 1 a 365.")] // Walidacja ilości dni
        public int RentalDays { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow; 
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; } 

        // Klucz obcy do Car
        public int CarId { get; set; }
        public Car? Car { get; set; } 

    }
}