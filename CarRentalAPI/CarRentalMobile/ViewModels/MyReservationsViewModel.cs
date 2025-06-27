using CarRentalMobile.Models;
using CarRentalMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel; // Nadal potrzebne dla ObservableObject
// using CommunityToolkit.Mvvm.Input; // Usunięte, bo nie używamy RelayCommand
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Windows.Input; // Dodaj to dla ICommand

namespace CarRentalMobile.ViewModels
{
    public partial class MyReservationsViewModel : ObservableObject
    {
        private readonly CarRentalApiService _apiService;

        [ObservableProperty]
        public ObservableCollection<Reservation> _reservations;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _emptyListMessage = "Brak dostępnych rezerwacji.";

        // Publiczna właściwość dla komendy
        public ICommand LoadReservationsCommand { get; } // Zmieniona nazwa, aby nie mylić z wygenerowaną

        public MyReservationsViewModel(CarRentalApiService apiService)
        {
            _apiService = apiService;
            Reservations = new ObservableCollection<Reservation>();
            Debug.WriteLine("MyReservationsViewModel constructor called.");

            // Inicjalizacja komendy
            LoadReservationsCommand = new Command(async () => await LoadReservationsAsync());
        }

        // Metoda do ładowania rezerwacji (pozostaje asynchroniczna)
        // Usunięto [RelayCommand]
        public async Task LoadReservationsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                Debug.WriteLine("LoadReservationsAsync: IsBusy set to true. Attempting to load reservations.");

                var fetchedReservations = await _apiService.GetReservationsAsync();

                Reservations.Clear();

                if (fetchedReservations != null)
                {
                    foreach (var reservation in fetchedReservations)
                    {
                        if (reservation.Car == null)
                        {
                            Debug.WriteLine($"UWAGA: Rezerwacja z ID {reservation.Id} ma Car == null!");
                            // Możesz też dodać inne działanie: pominięcie, wyrzucenie błędu, itp.
                        }
                        Reservations.Add(reservation);
                    }
                }

                EmptyListMessage = Reservations.Any() ? "" : "Brak dostępnych rezerwacji.";
                Debug.WriteLine($"LoadReservationsAsync: Loaded {Reservations.Count} reservations. EmptyListMessage: '{EmptyListMessage}'");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Błąd podczas ładowania rezerwacji: {ex.Message}");
                await Shell.Current.DisplayAlert("Błąd", $"Nie udało się załadować rezerwacji: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                Debug.WriteLine("LoadReservationsAsync: IsBusy set to false.");
            }
        }

        public void OnAppearing()
        {
            // Wywołujemy komendę
            LoadReservationsCommand.Execute(null);
            Debug.WriteLine("MyReservationsViewModel OnAppearing called. LoadReservationsCommand executed.");
        }
    }
}