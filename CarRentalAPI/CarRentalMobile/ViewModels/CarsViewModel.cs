using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input; // <-- Dodane: potrzebne do ICommand
using Newtonsoft.Json; // <-- Dodane: do serializacji auta do JSON
using CarRentalMobile.Models;
using CarRentalMobile.Services;

namespace CarRentalMobile.ViewModels
{
    [QueryProperty(nameof(CityId), "cityId")] // Umożliwia przekazywanie parametrów przez URL
    [QueryProperty(nameof(CityName), "cityName")] // Umożliwia przekazywanie parametrów przez URL
    public partial class CarsViewModel : ObservableObject
    {
        private readonly CarRentalApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<Car> _cars;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private int _cityId; // Właściwość do przechowywania ID miasta

        [ObservableProperty]
        private string _cityName; // Właściwość do przechowywania nazwy miasta dla wyświetlenia

        // 🔹 NOWOŚĆ: Komenda do przejścia na stronę rezerwacji auta
        public ICommand GoToReservationCommand { get; }

        public CarsViewModel(CarRentalApiService apiService)
        {
            _apiService = apiService;
            _cars = new ObservableCollection<Car>();
            LoadCarsCommand = new AsyncRelayCommand(LoadCarsAsync);

            // 🔹 NOWOŚĆ: Inicjalizacja komendy z metodą
            GoToReservationCommand = new Command<Car>(async (car) => await GoToReservationPage(car));
        }

        public IAsyncRelayCommand LoadCarsCommand { get; }

        // Metoda do ładowania samochodów (wywoływana ręcznie lub przez OnAppearing)
        private async Task LoadCarsAsync()
        {
            if (IsBusy)
                return;

            if (CityId == 0) // Upewnij się, że mamy ID miasta
                return;

            try
            {
                IsBusy = true;
                Cars.Clear();
                // Pobieramy samochody z API używając ID miasta
                var fetchedCars = await _apiService.GetCarsByCityIdAsync(CityId);

                foreach (var car in fetchedCars)
                {
                    Cars.Add(car);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        // 🔹 NOWOŚĆ: Przechodzi do ReservationPage z przekazanym JSONem auta
        private async Task GoToReservationPage(Car car)
        {
            if (car == null)
                return;

            var carJson = JsonConvert.SerializeObject(car); // Zamiana auta na JSON
            await Shell.Current.GoToAsync($"ReservationPage?carJson={Uri.EscapeDataString(carJson)}");
        }

        // Metoda, która zostanie wywołana po ustawieniu CityId przez QueryProperty
        partial void OnCityIdChanged(int value)
        {
            // Gdy CityId się zmieni, automatycznie ładujemy samochody
            if (LoadCarsCommand.CanExecute(null))
            {
                LoadCarsCommand.Execute(null);
            }
        }
    }
}

