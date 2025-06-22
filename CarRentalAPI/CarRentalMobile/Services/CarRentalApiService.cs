using CarRentalMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalMobile.Services
{
    public class CarRentalApiService
    {
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
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var cities = JsonConvert.DeserializeObject<ObservableCollection<City>>(json);
                return cities ?? new ObservableCollection<City>();
            }
            catch (HttpRequestException ex)
            {
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

        public async Task<ObservableCollection<Reservation>> GetReservationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}Reservations");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var reservations = JsonConvert.DeserializeObject<ObservableCollection<Reservation>>(json);
                return reservations ?? new ObservableCollection<Reservation>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Błąd HTTP podczas pobierania rezerwacji: {ex.Message}");
                return new ObservableCollection<Reservation>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Błąd deserializacji JSON dla rezerwacji: {ex.Message}");
                return new ObservableCollection<Reservation>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd podczas pobierania rezerwacji: {ex.Message}");
                return new ObservableCollection<Reservation>();
            }
        }

        public async Task<bool> CreateReservationAsync(Reservation reservation)
        {
            try
            {
                var json = JsonConvert.SerializeObject(reservation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}Reservations", content);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Błąd HTTP podczas tworzenia rezerwacji: {ex.Message}");
                return false;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Błąd serializacji JSON rezerwacji: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd podczas tworzenia rezerwacji: {ex.Message}");
                return false;
            }
        }
    }
}
