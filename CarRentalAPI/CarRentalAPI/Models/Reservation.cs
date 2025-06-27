using System;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; } // Klucz główny
        public decimal TotalCost { get; set; } // Całkowity koszt rezerwacji
        public string Status { get; set; } = "Pending"; // Status rezerwacji (np. Pending, Confirmed, Cancelled)

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Range(21, 120, ErrorMessage = "Wiek musi być między 21 a 120 lat.")] // Walidacja wieku
        public int Age { get; set; }

        [Range(1, 365, ErrorMessage = "Ilość dni rezerwacji musi być między 1 a 365.")] // Walidacja ilości dni
        public int RentalDays { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow; // Data rezerwacji
        public DateTime StartDate { get; set; } // Data rozpoczęcia rezerwacji
        public DateTime EndDate { get; set; } // Data zakończenia rezerwacji (StartDate + RentalDays)

        // Klucz obcy do Car
        public int CarId { get; set; }
        public Car? Car { get; set; } // Obiekt Car (relacja)

    }
}