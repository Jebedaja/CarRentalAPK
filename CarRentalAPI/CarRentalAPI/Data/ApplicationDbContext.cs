using CarRentalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Definicja tabel w bazie danych
        public DbSet<City> Cities { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        // Opcjonalnie: Konfiguracja relacji i danych początkowych
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasColumnType("decimal(18, 2)"); 


            // Konfiguracja klucza obcego dla Car
            modelBuilder.Entity<Car>()
                .HasOne(c => c.City)
                .WithMany(city => city.Cars)
                .HasForeignKey(c => c.CityId)
                .OnDelete(DeleteBehavior.Restrict); // Możesz wybrać Restrict lub Cascade

            // Konfiguracja klucza obcego dla Reservation
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Car)
                .WithMany(car => car.Reservations)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict);


            // Dodanie początkowych danych (seed data) - przydatne do testowania
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Warszawa", ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/images/warsaw.jpg" },
                new City { Id = 2, Name = "Kraków", ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/images/krakow.jpg" },
                new City { Id = 3, Name = "Gdańsk", ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/images/gdansk.jpg" }
            );

            modelBuilder.Entity<Car>().HasData(
                new Car { Id = 1, CityId = 1, Brand = "Toyota", Model = "Corolla", Year = 2022, PricePerDay = 150.00m, ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/images/corolla.jpg", IsAvailable = true },
                new Car { Id = 2, CityId = 1, Brand = "Skoda", Model = "Octavia", Year = 2023, PricePerDay = 180.00m, ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/images/octavia.jpg", IsAvailable = true },
                new Car { Id = 3, CityId = 2, Brand = "Volkswagen", Model = "Golf", Year = 2021, PricePerDay = 130.00m, ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/images/golf.jpg", IsAvailable = true },
                new Car { Id = 4, CityId = 3, Brand = "BMW", Model = "3 Series", Year = 2024, PricePerDay = 250.00m, ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/images/bmw3.jpg", IsAvailable = true }
            );
        }
    }
}