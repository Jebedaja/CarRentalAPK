using CarRentalMobile.Models;
using CarRentalMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace CarRentalMobile.ViewModels
{
    public partial class MyReservationsViewModel : ObservableObject
    {
        private readonly CarRentalApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<Reservation> reservations;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string emptyListMessage = "Brak dostępnych rezerwacji.";

        public ICommand LoadReservationsCommand { get; }
        public ICommand DeleteReservationCommand { get; }

        public MyReservationsViewModel(CarRentalApiService apiService) 
        {
            _apiService = apiService;

            Reservations = new ObservableCollection<Reservation>();

            LoadReservationsCommand = new Command(async () => await LoadReservationsAsync());  // ładowanie rezerwacji
            DeleteReservationCommand = new Command<Reservation>(async reservation =>
            {
                if (reservation == null)
                    return;

                bool confirm = await Shell.Current.DisplayAlert(  // potwiedzenie usuwania rezerwacji
                    "Usuń",
                    $"Na pewno usunąć rezerwację?",
                    "Tak",
                    "Nie");

                if (!confirm)
                    return;

                IsBusy = true;
                bool success = await _apiService.DeleteReservationAsync(reservation.Id);    // wywolanie usuwania rezzerwacji z api
                IsBusy = false;

                if (success)
                {
                    Reservations.Remove(reservation);
                    EmptyListMessage = Reservations.Any() ? "" : "Brak dostępnych rezerwacji.";
                }
                else
                {
                    await Shell.Current.DisplayAlert("Błąd", "Nie udało się usunąć rezerwacji.", "OK");
                }
            });
        }

        public async Task LoadReservationsAsync() 
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var fetched = await _apiService.GetReservationsAsync();  // pobieranie rezerwacji z api
                Reservations.Clear();

                if (fetched != null)
                {
                    foreach (var r in fetched)
                    {
                        Reservations.Add(r);
                    }
                }

                EmptyListMessage = Reservations.Any() ? "" : "Brak dostępnych rezerwacji.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Błąd podczas ładowania rezerwacji: {ex.Message}");
                await Shell.Current.DisplayAlert("Błąd", $"Nie udało się załadować rezerwacji: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            LoadReservationsCommand.Execute(null);
        }
    }
}
