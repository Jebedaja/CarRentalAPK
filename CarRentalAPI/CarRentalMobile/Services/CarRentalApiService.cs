using CarRentalMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json; 
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.Json; 

namespace CarRentalMobile.Services
{
    public class CarRentalApiService   // polaczenie z backendem
    {
        private readonly string _baseUrl = "https://carrentalbackend-ug-e9gtefcjaubeffev.germanywestcentral-01.azurewebsites.net/api/";
        private readonly HttpClient _httpClient;

        public CarRentalApiService()
        {
            _httpClient = new HttpClient();
        }

        // poniżej same metody do komunikacja z api od back
        //wszedzie async zeby nie blokowac intefesow 
        // jakies problemy z Car


        public async Task<ObservableCollection<City>> GetCitiesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}Cities");
                response.EnsureSuccessStatusCode();
                var cities = await response.Content.ReadFromJsonAsync<ObservableCollection<City>>();
                return cities ?? new ObservableCollection<City>();
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Błąd HTTP podczas pobierania miast: {ex.Message}");
                return new ObservableCollection<City>();
            }
            catch (JsonException ex) 
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
                var car = await response.Content.ReadFromJsonAsync<Car>();  // konwert odpowiedzi do obiektu Car
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
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Reservations", reservation);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Reservation>();  // deserializacja odpowiedzi do obiektu Reservation
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd podczas tworzenia rezerwacji: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> DeleteReservationAsync(int reservationId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}Reservations/{reservationId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Błąd podczas usuwania rezerwacji {reservationId}: {ex.Message}");
                return false;
            }
        }
        public async Task<ObservableCollection<Reservation>> GetReservationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}Reservations");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync(); 
                System.Diagnostics.Debug.WriteLine($"GetReservationsAsync - Raw JSON: {json}");

                var reservations = await response.Content.ReadFromJsonAsync<ObservableCollection<Reservation>>();

                System.Diagnostics.Debug.WriteLine($"GetReservationsAsync - Deserialized {reservations?.Count ?? 0} reservations.");   // logowanie ile bylo rezerwacji

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
                System.Diagnostics.Debug.WriteLine($"błąd podczas pobierania rezerwacji: {ex.Message}");
                return new ObservableCollection<Reservation>();
            }
        }
    }
}