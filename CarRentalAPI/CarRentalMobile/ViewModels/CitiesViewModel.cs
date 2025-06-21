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
    public partial class CitiesViewModel : ObservableObject
    {
        private readonly CarRentalApiService _apiService;

        // Właściwość do przechowywania listy miast
        [ObservableProperty]
        private ObservableCollection<City> _cities;

        // Właściwość do obsługi stanu ładowania danych
        [ObservableProperty]
        private bool _isBusy;

        public CitiesViewModel()
        {
            _apiService = new CarRentalApiService(); // Inicjalizuj serwis API
            _cities = new ObservableCollection<City>(); // Inicjalizuj kolekcję miast
            LoadCitiesCommand = new AsyncRelayCommand(LoadCitiesAsync); // Inicjalizuj komendę
        }

        // Komenda do ładowania miast
        public IAsyncRelayCommand LoadCitiesCommand { get; }

        private async Task LoadCitiesAsync()
        {
            if (IsBusy) // Zapobiega wielokrotnym wywołaniom, gdy już ładujemy dane
                return;

            try
            {
                IsBusy = true; // Ustaw flagę ładowania na true
                Cities.Clear(); // Wyczyść istniejącą listę przed załadowaniem nowych danych
                var fetchedCities = await _apiService.GetCitiesAsync(); // Pobierz miasta z API

                foreach (var city in fetchedCities)
                {
                    Cities.Add(city); // Dodaj każde miasto do kolekcji
                }
            }
            finally
            {
                IsBusy = false; // Ustaw flagę ładowania na false po zakończeniu
            }
        }
    }
}
