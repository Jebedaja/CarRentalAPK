using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CarRentalMobile.Models; // Pamiętaj o dodaniu using dla modeli
using CarRentalMobile.Services; // Pamiętaj o dodaniu using dla serwisu

namespace CarRentalMobile.ViewModels;

public partial class MyReservationsViewModel : ObservableObject
{
    // Na razie tylko podstawowe właściwości, rozbudujemy je później
    [ObservableProperty]
    private ObservableCollection<Reservation> _reservations;

    [ObservableProperty]
    private bool _isBusy;

    private readonly CarRentalApiService _apiService; // Deklaracja serwisu

    public MyReservationsViewModel() // Konstruktor bez parametrów dla XAML, jeśli chcesz
    {
        _reservations = new ObservableCollection<Reservation>();
        // _apiService = new CarRentalApiService(); // NIE RÓB TEGO, użyj wstrzykiwania zależności!
    }

    // Prawidłowy konstruktor do wstrzykiwania zależności
    public MyReservationsViewModel(CarRentalApiService apiService)
    {
        _apiService = apiService;
        _reservations = new ObservableCollection<Reservation>();
        // W przyszłości tutaj dodamy komendy do ładowania, usuwania, edycji rezerwacji
    }
}