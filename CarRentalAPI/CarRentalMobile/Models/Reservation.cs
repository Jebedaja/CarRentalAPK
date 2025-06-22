using System.ComponentModel.DataAnnotations;
using System;

namespace CarRentalMobile.Models
{
    public class Reservation
    {
        public int Id { get; set; } // Klucz główny
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
        public Car Car { get; set; } // Obiekt Car (relacja)
        public decimal TotalPrice => RentalDays * (Car?.DailyPrice ?? 0);


        // Klucz obcy do City (opcjonalnie, ale może być przydatne do filtrowania rezerwacji po mieście)
        // Możemy go dodać, jeśli uznamy to za potrzebne później. Na razie zostawimy tylko CarId,
        // a miasto pobierzemy przez Car.City.
        // public int CityId { get; set; }
        // public City City { get; set; }
    }
}
