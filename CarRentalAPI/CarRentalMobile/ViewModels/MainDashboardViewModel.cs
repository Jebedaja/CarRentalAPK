using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using CarRentalMobile.Views;

namespace CarRentalMobile.ViewModels;

public partial class MainDashboardViewModel : ObservableObject
{
    public MainDashboardViewModel()
    {
        // inicjalizacja jeśli potrzebna
    }

    [RelayCommand]
    async Task GoToCities()
    {
        // pozostaje bez zmian
        await Shell.Current.GoToAsync(nameof(CitiesPage));
    }

    [RelayCommand]
    async Task GoToReservations()
    {
        // absolutna nawigacja do ReservationsPage
        await Shell.Current.GoToAsync("///ReservationsPage");
    }
}
