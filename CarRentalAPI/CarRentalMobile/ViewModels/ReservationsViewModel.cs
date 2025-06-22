using CarRentalMobile.Models;
using CarRentalMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace CarRentalMobile.ViewModels;

// Pozwala przekazać JSON z danymi samochodu przy nawigacji
[QueryProperty(nameof(CarJson), "carJson")]
public partial class ReservationViewModel : ObservableObject
{
    private readonly CarRentalApiService _apiService;

    public ReservationViewModel()
    {
        _apiService = new CarRentalApiService();
        SubmitReservationCommand = new AsyncRelayCommand(SendReservationAsync);
    }

    // ================================
    // 🔽 Właściwości do bindowania
    // ================================

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

    // ================================
    // 🔘 Komenda do zatwierdzenia rezerwacji
    // ================================
    public IAsyncRelayCommand SubmitReservationCommand { get; }

    // ================================
    // 🪧 Tekst do nagłówka formularza
    // ================================
    public string CarDisplay => SelectedCar is null
        ? "Wybrany pojazd"
        : $"{SelectedCar.Brand} {SelectedCar.Model} ({SelectedCar.Year})";

    // ================================
    // 🔁 Automatyczna aktualizacja nagłówka po zmianie samochodu
    // ================================
    partial void OnSelectedCarChanged(Car? value)
    {
        OnPropertyChanged(nameof(CarDisplay));
    }

    // ================================
    // 📥 Deserializacja JSON z samochodem przy przejściu do widoku
    // ================================
    partial void OnCarJsonChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            try
            {
                SelectedCar = JsonConvert.DeserializeObject<Car>(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd deserializacji samochodu: {ex.Message}");
            }
        }
    }

    // ================================
    // 📤 Wysłanie rezerwacji do API
    // ================================
    private async Task SendReservationAsync()
    {
        // 👮‍♂️ Walidacje formularza
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

        // 📝 Przygotowanie danych do API
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

        Console.WriteLine("Rezerwacja do wysłania:");
        Console.WriteLine(JsonConvert.SerializeObject(reservation, Formatting.Indented));


        // 📡 Wysłanie POST do API
        var result = await _apiService.CreateReservationAsync(reservation);

        if (result)
        {
            await Shell.Current.DisplayAlert("Sukces", "Rezerwacja zakończona powodzeniem!", "OK");
            await Shell.Current.GoToAsync("//MainMenuPage");
        }
        else
        {
            await Shell.Current.DisplayAlert("Błąd", "Wystąpił problem przy rezerwacji.", "OK");
        }
    }
}



