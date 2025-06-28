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
    private readonly CarRentalApiService _apiService; //instacja sewisu api wrzyknieta z di

    [ObservableProperty]
    private ObservableCollection<Car> _cars;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _title; // nazwa miasta

    private int _cityId; 

    public int CityId
    {
        get => _cityId;
        set
        {
            if (SetProperty(ref _cityId, value))
            {
                LoadCarsCommand.Execute(null); //jesli city jest ustawione, ladowanie samochodów
            }
        }
    }

    public string CityName 
    {
        get => Title; 
        set => SetProperty(ref _title, value);
    }

    public CarsViewModel(CarRentalApiService apiService)
    {
        _apiService = apiService;
        Cars = new ObservableCollection<Car>();
    }

    [RelayCommand]
    async Task LoadCars()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;  // gdy isBusy jest true,laduje samoochody i nie można ladowac ponownie
            var cars = await _apiService.GetCarsByCityIdAsync(CityId);
            Cars.Clear();
            foreach (var car in cars)
            {
                Cars.Add(car);
            }
            Debug.WriteLine($"Zaladowano {Cars.Count} samochododow."); // Log
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Błąd", $"Nie udało się załadować samochodow: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    
    [RelayCommand]
    async Task GoToReservationDetails(Car selectedCar)  // obsluga wyboru samochodu
    {
        if (selectedCar == null)
        {
            Debug.WriteLine("Auto jest nullem."); //
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(ReservationDetailsPage)}?carId={selectedCar.Id}");
    }
}