using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CarsViewModel(CarRentalApiService apiService)
        {
            _apiService = apiService;
            _cars = new ObservableCollection<Car>();
            LoadCarsCommand = new AsyncRelayCommand(LoadCarsAsync);
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
