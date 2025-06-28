using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CarRentalMobile.Models;
using CarRentalMobile.Services;
using Microsoft.Maui.Controls; 

namespace CarRentalMobile.ViewModels
{
    public partial class CitiesViewModel : ObservableObject
    {
        private readonly CarRentalApiService _apiService; // api z di

        [ObservableProperty]
        private ObservableCollection<City> _cities;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanNavigateToCars))]
        private City selectedCity; // Pole do przechowywania wybranego miasta

        public IAsyncRelayCommand LoadCitiesCommand { get; }

        public bool CanNavigateToCars => SelectedCity != null; //sprawdza czy jest wybrane miasto, jak nie to null

        public CitiesViewModel(CarRentalApiService apiService)
        {
            _apiService = apiService;
            _cities = new ObservableCollection<City>();
            LoadCitiesCommand = new AsyncRelayCommand(LoadCitiesAsync);
        }

        [RelayCommand]
        private async Task GoToCars(City selectedCityFromTap) // przechodzenie do cars page z wybranego miasta
        {
            if (selectedCityFromTap == null) // jak miasto nie wybraneto nic nie robimy
                return;

            SelectedCity = selectedCityFromTap; // a jak wybrane to je ustawiamy

            await Shell.Current.GoToAsync($"CarsPage?cityId={selectedCityFromTap.Id}&cityName={selectedCityFromTap.Name}"); // przekazywanie id i nazwy miasta carspage

            SelectedCity = null;
        }

        private async Task LoadCitiesAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true; // zeby nie ladowal ponownie jak juz jest ladowane
                Cities.Clear();
                var fetchedCities = await _apiService.GetCitiesAsync();

                foreach (var city in fetchedCities)
                {
                    switch (city.Name.ToLower())
                    {
                        case "warszawa":                  //wspolrzędne
                            city.Latitude = 52.2297;  
                            city.Longitude = 21.0122;
                            break;
                        case "krakow":
                            city.Latitude = 50.0647;
                            city.Longitude = 19.9450;
                            break;
                        case "gdańsk":
                        case "gdansk":
                            city.Latitude = 54.3961; 
                            city.Longitude = 18.5977;
                            break;
                        case "mediolan":
                            city.Latitude = 45.2751; 
                            city.Longitude = 9.1122;
                            break;
                    }
                    Cities.Add(city);
                }

                await DetectAndShowLocationAsync(); //wykryj i pokaz lokalizacje
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task DetectAndShowLocationAsync()
        {
            try
            {
                var permissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (permissionStatus != PermissionStatus.Granted)
                {
                    permissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (permissionStatus != PermissionStatus.Granted)
                {
                    await Shell.Current.DisplayAlert("Błąd", "Brak zgody na dostęp do lokalizacji.", "OK"); // bez zgody nie przejdzie i nie dziala
                    return;
                }

                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium, // dokładność lokalizacji
                        Timeout = TimeSpan.FromSeconds(10) // czas czekania na lokalizacke
                    });
                }

                if (location != null)
                {
                    var nearestCity = FindNearestCity(location.Latitude, location.Longitude); 

                    if (nearestCity != null)
                    {
                        double distance = GetDistance(location.Latitude, location.Longitude, nearestCity.Latitude, nearestCity.Longitude); // liczenie odleglosci

                        await Shell.Current.DisplayAlert( // alert  z lokalizacja
                            "Lokalizacja",
                            $"Najbliższe miasto z flotą: {nearestCity.Name}\nOdległość: {distance:F1} km",
                            "OK"
                        );
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Błąd", "Nie znaleziono miasta z flotą.", "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Błąd", "Nie udało się uzyskać lokalizacji.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", $"Wystąpił problem z lokalizacją: {ex.Message}", "OK");
            }
        }

        private City? FindNearestCity(double lat, double lon)  // znajdowanie najbliższego miasta
        {
            City? nearestCity = null;
            double minDistance = double.MaxValue;

            foreach (var city in Cities)
            {
                if (city.Latitude != 0 && city.Longitude != 0)  // czy miasto ma wgl wspolrzedne
                {
                    var distance = GetDistance(lat, lon, city.Latitude, city.Longitude);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestCity = city;
                    }
                }
            }

            return nearestCity;
        }

        private double GetDistance(double lat1, double lon1, double lat2, double lon2) // funkcja do obliczania odległości między dwoma punktami na ziemi
        {
            const double R = 6371; // promien ziemi
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) * 
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // zwrot odległosci w kilometrach
        }

        private double DegreesToRadians(double deg) => deg * (Math.PI / 180);
    }
}