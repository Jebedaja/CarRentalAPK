using CarRentalMobile.Models;
using CarRentalMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Diagnostics; // Do Debug.WriteLine
using System.Text.Json; // Dodaj ten using!

namespace CarRentalMobile.ViewModels;

[QueryProperty(nameof(CarId), "carId")]
public partial class ReservationDetailsViewModel : ObservableObject
{
    private readonly CarRentalApiService _apiService;

    // Properties odbierane z QueryProperty
    private int _carId;
    public int CarId
    {
        get => _carId;
        set
        {
            if (SetProperty(ref _carId, value))
            {
                // Wywołaj komendę ładowania szczegółów samochodu po otrzymaniu CarId
                LoadCarDetailsCommand.Execute(null);
            }
        }
    }

    [ObservableProperty]
    private Car _selectedCar; // Wybrany samochód, którego szczegóły są wyświetlane

    // Pola formularza rezerwacji (wiązane z elementami UI)
    [ObservableProperty]
    private string _customerName;
    [ObservableProperty]
    private string _customerSurname;
    [ObservableProperty]
    private int _customerAge; // Wiek klienta
    [ObservableProperty]
    private int _numberOfDays; // Liczba dni wynajmu

    [ObservableProperty]
    private DateTime _startDate = DateTime.Today; // Domyślnie dzisiaj
    public DateTime MinimumStartDate => DateTime.Today; // Nie można rezerwować w przeszłości

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalPrice))] // Zaktualizuj TotalPrice, gdy endDate się zmieni
    private DateTime _endDate;

    [ObservableProperty]
    private bool _isBusy; // Do wskaźnika ładowania
    public bool IsNotBusy => !IsBusy; // Używane do włączania/wyłączania przycisku

    // Właściwość obliczeniowa dla całkowitej ceny
    public decimal TotalPrice
    {
        get
        {
            if (SelectedCar == null || NumberOfDays <= 0)
                return 0m;
            return SelectedCar.PricePerDay * NumberOfDays;
        }
    }

    // Konstruktor ViewModelu
    public ReservationDetailsViewModel(CarRentalApiService apiService)
    {
        _apiService = apiService;
        UpdateEndDateAndPrice(); // Inicjalne obliczenie daty zakończenia i ceny
        Debug.WriteLine("ReservationDetailsViewModel constructor called."); // Logowanie stworzenia ViewModelu
    }

    // Metody partial void On...Changed automatycznie generowane przez ObservableProperty
    // Wywołujemy w nich NotifyCanExecuteChanged() aby zaktualizować stan przycisku rezerwacji
    partial void OnCustomerNameChanged(string value) => MakeReservationCommand.NotifyCanExecuteChanged();
    partial void OnCustomerSurnameChanged(string value) => MakeReservationCommand.NotifyCanExecuteChanged();
    partial void OnCustomerAgeChanged(int value) => MakeReservationCommand.NotifyCanExecuteChanged();
    partial void OnNumberOfDaysChanged(int value) => UpdateEndDateAndPrice(); // Oblicza EndDate i TotalPrice
    partial void OnStartDateChanged(DateTime value) => UpdateEndDateAndPrice(); // Oblicza EndDate i TotalPrice


    // Metoda pomocnicza do obliczania daty zakończenia i całkowitej ceny
    private void UpdateEndDateAndPrice()
    {
        if (NumberOfDays > 0)
        {
            EndDate = StartDate.AddDays(NumberOfDays);
        }
        else
        {
            EndDate = StartDate; // Jeśli dni = 0, koniec to początek
        }

        OnPropertyChanged(nameof(TotalPrice)); // Powiadom o zmianie TotalPrice
        MakeReservationCommand.NotifyCanExecuteChanged(); // Zaktualizuj stan przycisku Zarezerwuj
        Debug.WriteLine($"UpdateEndDateAndPrice called. NumberOfDays: {NumberOfDays}, StartDate: {StartDate.ToShortDateString()}, EndDate: {EndDate.ToShortDateString()}, TotalPrice: {TotalPrice}. Command CanExecute should be notified.");
    }

    // Komenda do ładowania szczegółów samochodu po otrzymaniu CarId
    [RelayCommand]
    async Task LoadCarDetails()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Debug.WriteLine($"LoadCarDetails: IsBusy set to true. Loading car with ID: {CarId}");

            SelectedCar = await _apiService.GetCarByIdAsync(CarId); // Pobieramy szczegóły samochodu

            if (SelectedCar == null)
            {
                Debug.WriteLine("LoadCarDetails: SelectedCar is null. Displaying error and navigating back.");
                await Shell.Current.DisplayAlert("Błąd", "Nie udało się pobrać szczegółów samochodu.", "OK");
                await Shell.Current.GoToAsync(".."); // Wróć do poprzedniej strony
            }
            else
            {
                Debug.WriteLine($"LoadCarDetails: Car loaded: {SelectedCar.Brand} {SelectedCar.Model}.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Błąd ładowania szczegółów samochodu w LoadCarDetails: {ex.Message}");
            await Shell.Current.DisplayAlert("Błąd", $"Nie udało się załadować szczegółów samochodu: {ex.Message}", "OK");
            await Shell.Current.GoToAsync("..");
        }
        finally
        {
            IsBusy = false;
            MakeReservationCommand.NotifyCanExecuteChanged(); // Ważne: Po zakończeniu ładowania, zaktualizuj stan przycisku
            Debug.WriteLine("LoadCarDetails: IsBusy set to false, NotifyCanExecuteChanged called.");
        }
    }

    // Komenda do tworzenia rezerwacji
    [RelayCommand(CanExecute = nameof(CanMakeReservation))]
    async Task MakeReservation()
    {
        // TEN KOMUNIKAT POWINIEN SIĘ POJAWIĆ, JEŚLI COMMAND ZOSTAŁ WYKONANY (tj. CanExecute zwróciło true)
        Debug.WriteLine("MakeReservation command started.");

        if (IsBusy) return; // Jeśli już zajęty, wyjdź

        try
        {
            IsBusy = true; // Ustaw flagę zajętości
            Debug.WriteLine("MakeReservation: IsBusy set to true.");

            // Walidacja danych formularza
            if (SelectedCar == null || NumberOfDays <= 0 || CustomerAge < 21 || // Wiek musi być >= 21
                string.IsNullOrWhiteSpace(CustomerName) || string.IsNullOrWhiteSpace(CustomerSurname))
            {
                Debug.WriteLine("MakeReservation: Validation failed. Showing alert.");
                await Shell.Current.DisplayAlert("Błąd", "Wypełnij wszystkie pola poprawnie (Wiek min. 21 lat, Dni min. 1).", "OK");
                return; // Zatrzymaj wykonanie komendy, jeśli walidacja nie przeszła
            }
            Debug.WriteLine("MakeReservation: Validation passed. Creating new reservation object.");

            // Tworzenie obiektu rezerwacji do wysłania do API
            var newReservation = new Reservation
            {
                CarId = SelectedCar.Id,
                FirstName = CustomerName,
                LastName = CustomerSurname,
                Age = CustomerAge,
                RentalDays = NumberOfDays,
                ReservationDate = DateTime.UtcNow, // Ustawiamy datę rezerwacji na teraz (UTC)
                StartDate = StartDate.Date,
                EndDate = EndDate.Date,
                TotalCost = TotalPrice, // Całkowity koszt obliczony w ViewModelu
                Status = "Pending" // Domyślny status rezerwacji
            };

            // **************** NOWE LOGOWANIE JSON! ****************
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true }; // Dla ładnego formatowania JSON
            var jsonPayload = JsonSerializer.Serialize(newReservation, jsonOptions);
            Debug.WriteLine($"MakeReservation: Sending JSON Payload:\n{jsonPayload}");
            // ************************************************

            Debug.WriteLine($"Attempting to create reservation via API for CarId: {newReservation.CarId}, Customer: {newReservation.FirstName} {newReservation.LastName}, TotalCost: {newReservation.TotalCost}, Days: {newReservation.RentalDays}, StartDate: {newReservation.StartDate.ToShortDateString()}");

            // Wywołanie serwisu API do utworzenia rezerwacji
            var createdReservation = await _apiService.CreateReservationAsync(newReservation);
            Debug.WriteLine($"CreateReservationAsync returned: {(createdReservation != null ? "Success" : "Null/Failed by API")}");

            if (createdReservation != null)
            {
                Vibration.Vibrate(TimeSpan.FromMilliseconds(3000));

                await Shell.Current.DisplayAlert("Sukces", $"Rezerwacja dla {SelectedCar.Brand} {SelectedCar.Model} została pomyślnie utworzona!", "OK");
                // Po udanej rezerwacji, wróć do poprzedniej strony (np. listy samochodów)
                // Możesz też nawigować do strony "Moje rezerwacje"
                await Shell.Current.GoToAsync(".."); // Powrót na stronę listy samochodów
            }
            else
            {
                await Shell.Current.DisplayAlert("Błąd", "Nie udało się utworzyć rezerwacji. Spróbuj ponownie. Sprawdź logi debugowania.", "OK");
            }
        }
        catch (Exception ex) // Obsługa błędów, które mogą wystąpić podczas procesu rezerwacji
        {
            Debug.WriteLine($"Błąd podczas rezerwacji w MakeReservation (catch block): {ex.Message}");
            await Shell.Current.DisplayAlert("Błąd", $"Wystąpił błąd podczas tworzenia rezerwacji: {ex.Message}. Sprawdź połączenie z internetem lub dane.", "OK");
        }
        finally
        {
            IsBusy = false; // Zawsze zresetuj flagę zajętości
            MakeReservationCommand.NotifyCanExecuteChanged(); // Zawsze zaktualizuj stan przycisku po zakończeniu operacji
            Debug.WriteLine("MakeReservation: IsBusy set to false, NotifyCanExecuteChanged called in finally block.");
        }
    }

    // Metoda sprawdzająca, czy komenda MakeReservation może być wykonana (czy przycisk jest aktywny)
    bool CanMakeReservation()
    {
        Debug.WriteLine("CanMakeReservation: Temporarily returning TRUE for testing purposes.");
        return true; // Tymczasowo ustaw na true dla celów testowych
    }
}