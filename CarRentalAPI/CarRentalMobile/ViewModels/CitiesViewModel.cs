// ✅ Dodane dla lokalizacji:
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CarRentalMobile.Models;
using CarRentalMobile.Services;
using Microsoft.Maui.Controls; // Potrzebne do Shell.Current

namespace CarRentalMobile.ViewModels
{
    public partial class CitiesViewModel : ObservableObject
    {
        private readonly CarRentalApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<City> _cities;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanNavigateToCars))]
        private City selectedCity; // Pole dla SelectedCity

        // Komenda LoadCitiesCommand
        public IAsyncRelayCommand LoadCitiesCommand { get; }

        // Pomocnicza właściwość
        public bool CanNavigateToCars => SelectedCity != null;

        public CitiesViewModel(CarRentalApiService apiService)
        {
            _apiService = apiService;
            _cities = new ObservableCollection<City>();
            LoadCitiesCommand = new AsyncRelayCommand(LoadCitiesAsync);

            // selectedCity jest inicjalizowane na null domyślnie
        }

        // Automatycznie generuje GoToCarsCommand
        [RelayCommand]
        private async Task GoToCars(City selectedCityFromTap) // Nazwa parametru inna, by nie kolidowała z polem
        {
            if (selectedCityFromTap == null)
                return;

            // Ustaw SelectedCity (wygenerowaną właściwość) - to spowoduje aktualizację CanNavigateToCars
            SelectedCity = selectedCityFromTap; // Ustaw wygenerowaną właściwość

            await Shell.Current.GoToAsync($"CarsPage?cityId={selectedCityFromTap.Id}&cityName={selectedCityFromTap.Name}");

            SelectedCity = null;
        }

        private async Task LoadCitiesAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                Cities.Clear();
                var fetchedCities = await _apiService.GetCitiesAsync();

                // 🔽 PRZYKŁADOWE przypisanie współrzędnych – zakładamy, że nie przychodzą z backendu
                foreach (var city in fetchedCities)
                {
                    switch (city.Name.ToLower())
                    {
                        case "warszawa":
                            city.Latitude = 52.2297;
                            city.Longitude = 21.0122;
                            break;
                        case "krakow":
                            city.Latitude = 50.0647;
                            city.Longitude = 19.9450;
                            break;
                        case "gdańsk":
                        case "gdansk":
                            city.Latitude = 54.3961; // 📍Twoje dokładne współrzędne
                            city.Longitude = 18.5977;
                            break;
                    }
                    Cities.Add(city);
                }

                // 🔽 Dodano: po załadowaniu miast, uruchamiamy lokalizację
                await DetectAndShowLocationAsync();
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
                    await Shell.Current.DisplayAlert("Błąd", "Brak zgody na dostęp do lokalizacji.", "OK");
                    return;
                }

                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(10)
                    });
                }

                if (location != null)
                {
                    var nearestCity = FindNearestCity(location.Latitude, location.Longitude);

                    if (nearestCity != null)
                    {
                        double distance = GetDistance(location.Latitude, location.Longitude, nearestCity.Latitude, nearestCity.Longitude);

                        await Shell.Current.DisplayAlert(
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

        private City? FindNearestCity(double lat, double lon)
        {
            City? nearestCity = null;
            double minDistance = double.MaxValue;

            foreach (var city in Cities)
            {
                if (city.Latitude != 0 && city.Longitude != 0)
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

        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // promień Ziemi w km
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double DegreesToRadians(double deg) => deg * (Math.PI / 180);
    }
}