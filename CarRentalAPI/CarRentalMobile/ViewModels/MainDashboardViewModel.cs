using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel.Communication;
using CarRentalMobile.Views;
using Microsoft.Maui.ApplicationModel; // Dodaj ten using dla Permissions

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
        await Shell.Current.GoToAsync(nameof(CitiesPage));
    }

    [RelayCommand]
    async Task GoToReservations()
    {
        await Shell.Current.GoToAsync(nameof(ReservationsPage));
    }

    [RelayCommand]
    async Task CallUs() // Zmieniono na async Task
    {
        try
        {
            // 1. Sprawdź i zażądaj uprawnień do dzwonienia
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Phone>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Phone>();
            }

            if (status == PermissionStatus.Granted)
            {
                if (PhoneDialer.Default.IsSupported)
                {
                    PhoneDialer.Default.Open("123456789"); // <- wpisz tu swój numer
                }
                else
                {
                    // To jest raczej niemożliwe na prawdziwym telefonie z modemem
                    await Shell.Current.DisplayAlert("Błąd", "Funkcja dzwonienia nie jest obsługiwana na tym urządzeniu.", "OK");
                }
            }
            else
            {
                // Użytkownik odmówił uprawnień
                await Shell.Current.DisplayAlert("Błąd", "Brak uprawnień do dzwonienia. Aby zadzwonić, musisz zezwolić aplikacji na wykonywanie połączeń telefonicznych w ustawieniach urządzenia.", "OK");
            }
        }
        catch (FeatureNotSupportedException ex)
        {
            // Na przykład, jeśli aplikacja jest uruchomiona na tablecie bez modemu telefonicznego.
            await Shell.Current.DisplayAlert("Błąd", $"Dzwonienie nie jest obsługiwane na tym urządzeniu: {ex.Message}", "OK");
        }
        catch (Exception ex)
        {
            // Inny, nieoczekiwany błąd
            await Shell.Current.DisplayAlert("Błąd", $"Wystąpił nieoczekiwany błąd podczas próby dzwonienia: {ex.Message}", "OK");
        }
    }
}