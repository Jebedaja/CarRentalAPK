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
            // Ważne, aby upewnić się, że GoToCarsCommand.CanExecuteChanged jest wywołane
            // ale w tym scenariuszu, po wykonaniu komendy, zwykle nie ma to wpływu na wizualny stan elementu listy
            SelectedCity = selectedCityFromTap; // Ustaw wygenerowaną właściwość

            await Shell.Current.GoToAsync($"CarsPage?cityId={selectedCityFromTap.Id}&cityName={selectedCityFromTap.Name}");

            // Możesz zresetować selectedCity po nawigacji, jeśli chcesz, aby element nie był już "wybrany"
            // jeśli nie ma to znaczenia dla UI, można pominąć.
            SelectedCity = null;
        }

        // ... (reszta metody LoadCitiesAsync bez zmian)
        private async Task LoadCitiesAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                Cities.Clear();
                var fetchedCities = await _apiService.GetCitiesAsync();

                foreach (var city in fetchedCities)
                {
                    Cities.Add(city);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}