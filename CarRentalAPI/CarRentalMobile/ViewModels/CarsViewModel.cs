using CarRentalMobile.Models;
using CarRentalMobile.Services;
using CarRentalMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; // Upewnij się, że to jest tutaj

namespace CarRentalMobile.ViewModels;

[QueryProperty(nameof(CityId), "cityId")]
[QueryProperty(nameof(CityName), "cityName")]
public partial class CarsViewModel : ObservableObject
{
    private readonly CarRentalApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<Car> _cars;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _title; // Będzie to nazwa miasta

    private int _cityId; // Pole prywatne dla QueryProperty

    public int CityId
    {
        get => _cityId;
        set
        {
            if (SetProperty(ref _cityId, value))
            {
                // Gdy CityId jest ustawione, ładujemy samochody
                LoadCarsCommand.Execute(null);
            }
        }
    }

    public string CityName // Pole publiczne dla QueryProperty
    {
        get => Title; // Title będzie używane jako nazwa miasta na górze strony
        set => SetProperty(ref _title, value);
    }

    public CarsViewModel(CarRentalApiService apiService)
    {
        _apiService = apiService;
        Cars = new ObservableCollection<Car>();
        Debug.WriteLine("CarsViewModel constructor called."); // Dodatkowy log w konstruktorze
    }

    [RelayCommand]
    async Task LoadCars()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Debug.WriteLine($"LoadCars: IsBusy set to true. Loading cars for CityId: {CityId}"); // Log
            var cars = await _apiService.GetCarsByCityIdAsync(CityId);
            Cars.Clear();
            foreach (var car in cars)
            {
                Cars.Add(car);
            }
            Debug.WriteLine($"LoadCars: Loaded {Cars.Count} cars."); // Log
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Błąd ładowania samochodów w LoadCars: {ex.Message}");
            await Shell.Current.DisplayAlert("Błąd", $"Nie udało się załadować samochodów: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            Debug.WriteLine("LoadCars: IsBusy set to false."); // Log
        }
    }

    // NOWA KOMENDA: Obsługa wyboru samochodu z listy
    [RelayCommand]
    async Task GoToReservationDetails(Car selectedCar)
    {
        if (selectedCar == null)
        {
            Debug.WriteLine("GoToReservationDetails: selectedCar is null. Aborting navigation."); // Log
            return;
        }

        // TUTAJ DODAJEMY LOGA
        Debug.WriteLine($"GoToReservationDetails: Navigating to ReservationDetailsPage with CarId: {selectedCar.Id}");

        // Przekazujemy ID samochodu do ReservationDetailsPage
        await Shell.Current.GoToAsync($"{nameof(ReservationDetailsPage)}?carId={selectedCar.Id}");
    }
}