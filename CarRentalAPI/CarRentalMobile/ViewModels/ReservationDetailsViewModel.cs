using CarRentalMobile.Models;
using CarRentalMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Diagnostics; 
using System.Text.Json; 

namespace CarRentalMobile.ViewModels;

[QueryProperty(nameof(CarId), "carId")]
public partial class ReservationDetailsViewModel : ObservableObject
{
    private readonly CarRentalApiService _apiService;

    //  odbierane z queryProperty
    private int _carId;
    public int CarId
    {
        get => _carId;
        set
        {
            if (SetProperty(ref _carId, value))
            {
                LoadCarDetailsCommand.Execute(null); // jak dostanie id to ładowanie szczegółów samochodu
            }
        }
    }

    [ObservableProperty]
    private Car _selectedCar; // Wybrany samochód

    // Pola formularza rezerwacji 
    [ObservableProperty]
    private string _customerName;
    [ObservableProperty]
    private string _customerSurname;
    [ObservableProperty]
    private int _customerAge; 
    [ObservableProperty]
    private int _numberOfDays; // liczba dni wynajmy

    [ObservableProperty]
    private DateTime _startDate = DateTime.Today; // Domyślnie dzisiaj
    public DateTime MinimumStartDate => DateTime.Today; // Nie można rezerwować w przeszłości

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalPrice))] // Zaktualizuj TotalPrice, gdy endDate się zmieni
    private DateTime _endDate;

    [ObservableProperty]
    private bool _isBusy; 
    public bool IsNotBusy => !IsBusy; 

    
    public decimal TotalPrice // Całkowita cena rezerwacji
    {
        get
        {
            if (SelectedCar == null || NumberOfDays <= 0)
                return 0m;
            return SelectedCar.PricePerDay * NumberOfDays;
        }
    }

    public ReservationDetailsViewModel(CarRentalApiService apiService)
    {
        _apiService = apiService;
        UpdateEndDateAndPrice(); // Inicjalne obliczenie daty zakończenia i ceny
    }

    // Metody partial void On...Changed automatycznie generowane przez ObservableProperty
    partial void OnCustomerNameChanged(string value) => MakeReservationCommand.NotifyCanExecuteChanged(); // notifyCan..informuje o zmianie stanu buttona
    partial void OnCustomerSurnameChanged(string value) => MakeReservationCommand.NotifyCanExecuteChanged();
    partial void OnCustomerAgeChanged(int value) => MakeReservationCommand.NotifyCanExecuteChanged();
    partial void OnNumberOfDaysChanged(int value) => UpdateEndDateAndPrice(); 
    partial void OnStartDateChanged(DateTime value) => UpdateEndDateAndPrice(); // Oblicza date końcową i kwote lączna


    private void UpdateEndDateAndPrice()  // obliczanie daty końcowej
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
            MakeReservationCommand.NotifyCanExecuteChanged(); // Po zakończeniu ładowania, zaktualizuj stan przycisku
            Debug.WriteLine("LoadCarDetails: IsBusy set to false, NotifyCanExecuteChanged called.");
        }
    }

    // Komenda do tworzenia rezerwacji
    [RelayCommand(CanExecute = nameof(CanMakeReservation))]
    async Task MakeReservation()
    {
        if (IsBusy) return; // Jeśli już zajęty, wyjdź

        try
        {
            IsBusy = true;
            Debug.WriteLine("MakeReservation: IsBusy set to true.");

            // Walidacja danych formularza
            if (SelectedCar == null || NumberOfDays <= 0 || CustomerAge < 21 || // Wiek musi być >= 21
                string.IsNullOrWhiteSpace(CustomerName) || string.IsNullOrWhiteSpace(CustomerSurname))
            {
                await Shell.Current.DisplayAlert("Błąd", "Wypełnij wszystkie pola wiek i liczba dni wynajmu poprawnie (Wiek min. 21 lat, Dni min. 1).", "OK");
                return; // jak nie przejdzie walidacji to wychodzi
            }

            // Tworzenie obiektu rezerwacji do wysłania do API
            var newReservation = new Reservation
            {
                CarId = SelectedCar.Id,
                FirstName = CustomerName,
                LastName = CustomerSurname,
                Age = CustomerAge,
                RentalDays = NumberOfDays,
                ReservationDate = DateTime.UtcNow, // Ustawiamy datę rezerwacji na teraz UTC
                StartDate = StartDate.Date,
                EndDate = EndDate.Date,
                TotalCost = TotalPrice, // Całkowity koszt obliczony w ViewModelu
                Status = "Pending" // yy
            };

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true }; // opcje serializacji
            var jsonPayload = JsonSerializer.Serialize(newReservation, jsonOptions);

            // Wywołanie serwisu API do utworzenia rezerwacji
            var createdReservation = await _apiService.CreateReservationAsync(newReservation);

            if (createdReservation != null)
            {
                Vibration.Vibrate(TimeSpan.FromMilliseconds(3000));            // TUTAJ VIBRATION   <<-------------------

                await Shell.Current.DisplayAlert("Sukces", $"Rezerwacja dla {SelectedCar.Brand} {SelectedCar.Model} została pomyślnie utworzona!", "OK");
                await Shell.Current.GoToAsync(".."); // Powrót na stronę listy samochodów
            }
            else
            {
                await Shell.Current.DisplayAlert("Błąd", "Nie udało się utworzyć rezerwacji. Spróbuj ponownie", "OK");
            }
        }
        catch (Exception ex) // Obsługa błędów, które mogą wystąpić podczas procesu rezerwacji
        {
            await Shell.Current.DisplayAlert("Błąd", $"Wystąpił błąd podczas tworzenia rezerwacji: {ex.Message}. Sprawdź połączenie z internetem lub dane.", "OK");
        }
        finally
        {
            IsBusy = false; 
            MakeReservationCommand.NotifyCanExecuteChanged(); // aktualizacja stanu przycisku po zakończeniu operacji
        }
    }

    bool CanMakeReservation() 
    {
        return true; // no potem to lepiej taa
    }
}