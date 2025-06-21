using CarRentalMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace CarRentalMobile.Services
{
    public class CarRentalApiService
    {
        // WAŻNE: Zmień ten URL na URL Twojego wdrożonego API na Azure!
        // Użyj tego dłuższego, który działa ze /swagger
        private readonly string _baseUrl = "https://carrentalbackend-ug-e9gtefcjaubeffev.germanywestcentral-01.azurewebsites.net/api/";
        private readonly HttpClient _httpClient;

        public CarRentalApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ObservableCollection<City>> GetCitiesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}Cities");
                response.EnsureSuccessStatusCode(); // Sprawdza, czy kod statusu odpowiedzi to 2xx
                var json = await response.Content.ReadAsStringAsync();
                var cities = JsonConvert.DeserializeObject<ObservableCollection<City>>(json);
                return cities ?? new ObservableCollection<City>(); // Zwraca pustą kolekcję, jeśli deserializacja zwróci null
            }
            catch (HttpRequestException ex)
            {
                // Tutaj możesz logować błędy lub wyświetlać komunikaty użytkownikowi
                Console.WriteLine($"Błąd HTTP podczas pobierania miast: {ex.Message}");
                return new ObservableCollection<City>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Błąd deserializacji JSON dla miast: {ex.Message}");
                return new ObservableCollection<City>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd podczas pobierania miast: {ex.Message}");
                return new ObservableCollection<City>();
            }
        }

        public async Task<ObservableCollection<Car>> GetCarsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}Cars");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var cars = JsonConvert.DeserializeObject<ObservableCollection<Car>>(json);
                return cars ?? new ObservableCollection<Car>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Błąd HTTP podczas pobierania samochodów: {ex.Message}");
                return new ObservableCollection<Car>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Błąd deserializacji JSON dla samochodów: {ex.Message}");
                return new ObservableCollection<Car>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd podczas pobierania samochodów: {ex.Message}");
                return new ObservableCollection<Car>();
            }
        }

        public async Task<ObservableCollection<Car>> GetCarsByCityIdAsync(int cityId)
        {
            try
            {
                // Zakładamy, że w Twoim API istnieje taki endpoint.
                // Jeśli nie, będziemy musieli dodać go do CarRentalAPI/Controllers/CarsController
                var response = await _httpClient.GetAsync($"{_baseUrl}Cars/ByCity/{cityId}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var cars = JsonConvert.DeserializeObject<ObservableCollection<Car>>(json);
                return cars ?? new ObservableCollection<Car>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Błąd HTTP podczas pobierania samochodów dla miasta {cityId}: {ex.Message}");
                return new ObservableCollection<Car>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Błąd deserializacji JSON dla samochodów dla miasta {cityId}: {ex.Message}");
                return new ObservableCollection<Car>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd podczas pobierania samochodów dla miasta {cityId}: {ex.Message}");
                return new ObservableCollection<Car>();
            }
        }

        // Możesz tutaj dodawać inne metody, np. do tworzenia rezerwacji, pobierania rezerwacji itp.
        // public async Task<bool> CreateReservationAsync(Reservation reservation) { ... }
    }
}
