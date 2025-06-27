using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using CarRentalMobile.Views;

namespace CarRentalMobile.ViewModels;

public partial class MainDashboardViewModel : ObservableObject
{
    public MainDashboardViewModel()
    {
        // Tutaj możesz dodać logikę inicjalizacji, jeśli będzie potrzebna
    }

    [RelayCommand]
    async Task GoToCities()
    {
        await Shell.Current.GoToAsync(nameof(CitiesPage)); // Nawigacja do CitiesPage
    }

    [RelayCommand]
    async Task GoToMyReservations()
    {
        await Shell.Current.GoToAsync(nameof(MyReservationsPage)); // Nawigacja do MyReservationsPage
    }

    [RelayCommand]
    async Task GoToReservations()
    {
        // Nawigacja do strony Moich Rezerwacji
        await Shell.Current.GoToAsync(nameof(MyReservationsPage));
        //System.Diagnostics.Debug.WriteLine("Navigating to MyReservationsPage.");
    }
}