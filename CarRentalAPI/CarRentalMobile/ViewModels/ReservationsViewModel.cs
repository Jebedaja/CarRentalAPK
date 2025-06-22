using CarRentalMobile.Models;
using CarRentalMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Windows.Input;

namespace CarRentalMobile.ViewModels;

[QueryProperty(nameof(CarJson), "carJson")]
public partial class ReservationViewModel : ObservableObject
{
    private readonly CarRentalApiService _apiService;

    public ReservationViewModel()
    {
        _apiService = new CarRentalApiService();
        SubmitReservationCommand = new AsyncRelayCommand(SendReservationAsync);
    }

    [ObservableProperty]
    private string carJson;

    [ObservableProperty]
    private string firstName;

    [ObservableProperty]
    private string lastName;

    [ObservableProperty]
    private int age;

    [ObservableProperty]
    private int rentalDays;

    [ObservableProperty]
    private Car selectedCar;

    public IAsyncRelayCommand SubmitReservationCommand { get; }

    public string CarDisplay => selectedCar != null
        ? $"{selectedCar.Brand} {selectedCar.Model} ({selectedCar.Year})"
        : "Wybrany pojazd";

    partial void OnCarJsonChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            SelectedCar = JsonConvert.DeserializeObject<Car>(value);
            OnPropertyChanged(nameof(CarDisplay));
        }
    }

    private async Task SendReservationAsync()
    {
        if (Age < 21)
        {
            await Shell.Current.DisplayAlert("Błąd", "Musisz mieć co najmniej 21 lat", "OK");
            return;
        }

        if (RentalDays <= 0)
        {
            await Shell.Current.DisplayAlert("Błąd", "Liczba dni musi być większa niż 0", "OK");
            return;
        }

        var reservation = new Reservation
        {
            FirstName = FirstName,
            LastName = LastName,
            Age = Age,
            RentalDays = RentalDays,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(RentalDays),
            CarId = SelectedCar.Id
        };

        var result = await _apiService.CreateReservationAsync(reservation);

        if (result)
        {
            await Shell.Current.DisplayAlert("Sukces", "Rezerwacja zakończona powodzeniem!", "OK");
            await Shell.Current.GoToAsync("//MainMenuPage");
        }
        else
        {
            await Shell.Current.DisplayAlert("Błąd", "Wystąpił problem przy rezerwacji", "OK");
        }
    }
}


