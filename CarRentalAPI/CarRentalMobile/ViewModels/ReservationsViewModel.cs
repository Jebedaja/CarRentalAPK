using CarRentalMobile.Models;
using CarRentalMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CarRentalMobile.ViewModels
{
    public partial class ReservationsViewModel : ObservableObject
    {
        private readonly CarRentalApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<Reservation> reservations;

        [ObservableProperty]
        private bool isBusy;

        public ReservationsViewModel(CarRentalApiService apiService)
        {
            _apiService = apiService;
            Reservations = new ObservableCollection<Reservation>();
            LoadReservationsCommand = new AsyncRelayCommand(LoadReservationsAsync);
        }

        public IAsyncRelayCommand LoadReservationsCommand { get; }

        private async Task LoadReservationsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                Reservations.Clear();
                var fetched = await _apiService.GetReservationsAsync(); // Musimy tę metodę stworzyć!
                foreach (var reservation in fetched)
                    Reservations.Add(reservation);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

