using CarRentalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // jakie tabele w bazie
        public DbSet<City> Cities { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasColumnType("decimal(18, 2)"); 


           
            modelBuilder.Entity<Car>() 
                .HasOne(c => c.City)
                .WithMany(city => city.Cars)
                .HasForeignKey(c => c.CityId) // klucz obcy
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Car)
                .WithMany(car => car.Reservations)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict);



            // seedowanie 
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Warszawa", ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/obrazki/warszawa.jpg" },
                new City { Id = 2, Name = "Kraków", ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/obrazki/krak%C3%B3w.jpg" },
                new City { Id = 3, Name = "Gdańsk", ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/obrazki/Gdansk.jpg" },
                new City { Id = 4, Name = "Mediolan", ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/obrazki/Mediolan.jpg" }
            );

            modelBuilder.Entity<Car>().HasData(
                new Car { Id = 1, CityId = 1, Brand = "Toyota", Model = "Corolla", Year = 2022, PricePerDay = 150.00m, ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/obrazki/Corolla.jpg", IsAvailable = true },
                new Car { Id = 2, CityId = 1, Brand = "Skoda", Model = "Octavia", Year = 2023, PricePerDay = 180.00m, ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/obrazki/Octavia.jpg", IsAvailable = true },
                new Car { Id = 3, CityId = 2, Brand = "Volkswagen", Model = "Golf", Year = 2021, PricePerDay = 130.00m, ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/obrazki/Golf.jpg", IsAvailable = true },
                new Car { Id = 4, CityId = 3, Brand = "BMW", Model = "3 Series", Year = 2024, PricePerDay = 250.00m, ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/obrazki/bmw3.jpg", IsAvailable = true },
                new Car { Id = 5, CityId = 4, Brand = "Fiat", Model = "500", Year = 2022, PricePerDay = 130.00m, ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/obrazki/Fiat.jpg", IsAvailable = true },
                new Car { Id = 6, CityId = 4, Brand = "Setra", Model = "Inter", Year = 2011, PricePerDay = 1500.00m, ImageUrl = "https://carrentalapkstorage.blob.core.windows.net/obrazki/autokar_mediolan.jpg", IsAvailable = true }
            );
        }
    }
}