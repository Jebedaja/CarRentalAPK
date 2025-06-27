using CarRentalMobile.Models;
// Usunięto: using Newtonsoft.Json; // <-- USUWAMY TĄ LINIĘ
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json; // Używamy tego do ReadFromJsonAsync i PostAsJsonAsync
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.Json; // <-- DODAJEMY TEN USING, jeśli będziesz chciał ręcznie opcje serializera

namespace CarRentalMobile.Services
{
    public class CarRentalApiService
    {
        private readonly string _baseUrl = "https://carrentalbackend-ug-e9gtefcjaubeffev.germanywestcentral-01.azurewebsites.net/api/";
        private readonly HttpClient _httpClient;

        // Opcjonalnie: Jeśli potrzebujesz niestandardowych opcji dla System.Text.Json (np. CamelCasePropertyNamingPolicy)
        // private readonly JsonSerializerOptions _jsonSerializerOptions;

        public CarRentalApiService()
        {
            _httpClient = new HttpClient();
            // Jeśli potrzebujesz niestandardowych opcji, np. do obsługi nazw właściwości (camelCase vs PascalCase)
            // _jsonSerializerOptions = new JsonSerializerOptions
            // {
            //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Domyślnie w ASP.NET Core jest camelCase
            //     ReferenceHandler = ReferenceHandler.IgnoreCycles // To może być przydatne, jeśli kiedyś będziesz serializować dane na froncie
            // };
        }

        public async Task<ObservableCollection<City>> GetCitiesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}Cities");
                response.EnsureSuccessStatusCode();
                // Zmieniono deserializację na System.Net.Http.Json.ReadFromJsonAsync
                var cities = await response.Content.ReadFromJsonAsync<ObservableCollection<City>>();
                return cities ?? new ObservableCollection<City>();
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Błąd HTTP podczas pobierania miast: {ex.Message}");
                return new ObservableCollection<City>();
            }
            catch (JsonException ex) // Catch JsonException dla System.Text.Json
            {
                Debug.WriteLine($"Błąd deserializacji JSON dla miast: {ex.Message}");
                return new ObservableCollection<City>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Nieoczekiwany błąd podczas pobierania miast: {ex.Message}");
                return new ObservableCollection<City>();
            }
        }

        public async Task<ObservableCollection<Car>> GetCarsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}Cars");
                response.EnsureSuccessStatusCode();
                // Zmieniono deserializację
                var cars = await response.Content.ReadFromJsonAsync<ObservableCollection<Car>>();
                return cars ?? new ObservableCollection<Car>();
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Błąd HTTP podczas pobierania samochodów: {ex.Message}");
                return new ObservableCollection<Car>();
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"Błąd deserializacji JSON dla samochodów: {ex.Message}");
                return new ObservableCollection<Car>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Nieoczekiwany błąd podczas pobierania samochodów: {ex.Message}");
                return new ObservableCollection<Car>();
            }
        }

        public async Task<ObservableCollection<Car>> GetCarsByCityIdAsync(int cityId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}Cars/ByCity/{cityId}");
                response.EnsureSuccessStatusCode();
                // Zmieniono deserializację
                var cars = await response.Content.ReadFromJsonAsync<ObservableCollection<Car>>();
                return cars ?? new ObservableCollection<Car>();
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Błąd HTTP podczas pobierania samochodów dla miasta {cityId}: {ex.Message}");
                return new ObservableCollection<Car>();
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"Błąd deserializacji JSON dla samochodów dla miasta {cityId}: {ex.Message}");
                return new ObservableCollection<Car>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Nieoczekiwany błąd podczas pobierania samochodów dla miasta {cityId}: {ex.Message}");
                return new ObservableCollection<Car>();
            }
        }

        public async Task<Car> GetCarByIdAsync(int carId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}Cars/{carId}");
                response.EnsureSuccessStatusCode();
                // Zmieniono deserializację
                var car = await response.Content.ReadFromJsonAsync<Car>();
                return car;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Błąd HTTP podczas pobierania samochodu o ID {carId}: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"Błąd deserializacji JSON dla samochodu o ID {carId}: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Nieoczekiwany błąd podczas pobierania samochodu o ID {carId}: {ex.Message}");
                return null;
            }
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            try
            {
                // To już używa PostAsJsonAsync, które domyślnie korzysta z System.Text.Json
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Reservations", reservation);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Reservation>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd podczas tworzenia rezerwacji: {ex.Message}");
                return null;
            }
        }

        public async Task<ObservableCollection<Reservation>> GetReservationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}Reservations");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync(); // Możesz zostawić dla debugu
                System.Diagnostics.Debug.WriteLine($"GetReservationsAsync - Raw JSON: {json}");

                // ZMIEŃ TO: Użyj System.Text.Json
                var reservations = await response.Content.ReadFromJsonAsync<ObservableCollection<Reservation>>();

                System.Diagnostics.Debug.WriteLine($"GetReservationsAsync - Deserialized {reservations?.Count ?? 0} reservations.");

                return reservations ?? new ObservableCollection<Reservation>();
            }
            catch (HttpRequestException httpEx)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd HTTP podczas pobierania rezerwacji: {httpEx.StatusCode} - {httpEx.Message}");
                return new ObservableCollection<Reservation>();
            }
            catch (JsonException jsonEx)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd deserializacji JSON dla rezerwacji: {jsonEx.Message}");
                return new ObservableCollection<Reservation>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Nieoczekiwany błąd podczas pobierania rezerwacji: {ex.Message}");
                return new ObservableCollection<Reservation>();
            }
        }
    }
}