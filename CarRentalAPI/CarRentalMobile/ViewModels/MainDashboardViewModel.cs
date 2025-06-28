using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel.Communication;
using CarRentalMobile.Views;
using Microsoft.Maui.ApplicationModel; 

namespace CarRentalMobile.ViewModels;

public partial class MainDashboardViewModel : ObservableObject
{
    public MainDashboardViewModel()
    {
       
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
    async Task CallUs() 
    {
        try
        {
            
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Phone>(); // czy jest uprawnienie do dzwonienia

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Phone>();
            }

            if (status == PermissionStatus.Granted)
            {
                if (PhoneDialer.Default.IsSupported)
                {
                    PhoneDialer.Default.Open("123456789"); // numer przykladowy
                }
                else
                {
                    await Shell.Current.DisplayAlert("Błąd", "Funkcja dzwonienia nie dziala", "OK");
                }
            }
            else
            {
                // jak odmowo uprawnien
                await Shell.Current.DisplayAlert("Błąd", "Brak uprawnień do dzwonienia. zewzwól aby kontynuować", "OK");
            }
        }
        catch (FeatureNotSupportedException ex)
        {
            // to chyba glownie jak jest urządzenie bez funkcji dzwonienia np tablet
            await Shell.Current.DisplayAlert("Błąd", $"Dzwonienie nie jest obsługiwane na tym urządzeniu: {ex.Message}", "OK");
        }
        catch (Exception ex)
        {
            // Inne błądy
            await Shell.Current.DisplayAlert("Błąd", $"Wystąpił nieoczekiwany błąd podczas próby dzwonienia: {ex.Message}", "OK");
        }
    }
}