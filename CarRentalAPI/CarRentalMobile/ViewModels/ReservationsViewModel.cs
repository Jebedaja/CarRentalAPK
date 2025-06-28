using CarRentalMobile.Models;
using CarRentalMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;  

namespace CarRentalMobile.ViewModels;

[QueryProperty(nameof(CarJson), "carJson")]
public partial class ReservationViewModel : ObservableObject
{
    private readonly CarRentalApiService _apiService;

    public ReservationViewModel()
    {
        _apiService = new CarRentalApiService();
        SubmitReservationCommand = new AsyncRelayCommand(SendReservationAsync);  //wysylanie rezerwacji
    }

    [ObservableProperty]
    private string? carJson;

    [ObservableProperty]
    private string? firstName;

    [ObservableProperty]
    private string? lastName;

    [ObservableProperty]
    private int age;

    [ObservableProperty]
    private int rentalDays;

    [ObservableProperty]
    private Car? selectedCar;

    public IAsyncRelayCommand SubmitReservationCommand { get; }

    public string CarDisplay => SelectedCar is null  // jeśli nie wybrano samochodu
        ? "Wybrany pojazd"
        : $"{SelectedCar.Brand} {SelectedCar.Model} ({SelectedCar.Year})"; // info o samochodzie

    partial void OnSelectedCarChanged(Car? value)
    {
        OnPropertyChanged(nameof(CarDisplay));    // update info o samochodzie
    }

    partial void OnCarJsonChanged(string value)  
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            try
            {
                SelectedCar = JsonConvert.DeserializeObject<Car>(value);    // deserializacja do obiektu car
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd deserializacji samochodu: {ex.Message}");
            }
        }
    }

    private async Task SendReservationAsync() // wysyłanie rezerwacji do API
    {
        // walidacja
        if (SelectedCar == null)
        {
            await Shell.Current.DisplayAlert("Błąd", "Nie wybrano pojazdu.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
        {
            await Shell.Current.DisplayAlert("Błąd", "Uzupełnij imię i nazwisko.", "OK");
            return;
        }

        if (Age < 21)
        {
            await Shell.Current.DisplayAlert("Błąd", "Musisz mieć co najmniej 21 lat.", "OK");
            return;
        }

        if (RentalDays <= 0)
        {
            await Shell.Current.DisplayAlert("Błąd", "Liczba dni musi być większa niż 0.", "OK");
            return;
        }

        // obiekt rezerwacji
        var reservation = new Reservation
        {
            FirstName = FirstName!,
            LastName = LastName!,
            Age = Age,
            RentalDays = RentalDays,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(RentalDays),
            CarId = SelectedCar.Id
        };

        try
        {
            // zwróci obiekt Reservation lub null
            var result = await _apiService.CreateReservationAsync(reservation);

            if (result != null)
            {
                // sukces
                await Shell.Current.DisplayAlert("Sukces", "Rezerwacja zakończona powodzeniem!", "OK");
                await Shell.Current.GoToAsync("//MainMenuPage");
            }
            else
            {
                // API zwróciło null
                await Shell.Current.DisplayAlert("Błąd", "Wystąpił problem przy rezerwacji.", "OK");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[EXCEPTION] Wystąpił błąd: {ex}");
            await Shell.Current.DisplayAlert("Błąd", $"Nie udało się utworzyć rezerwacji: {ex.Message}", "OK");
        }
    }
}

