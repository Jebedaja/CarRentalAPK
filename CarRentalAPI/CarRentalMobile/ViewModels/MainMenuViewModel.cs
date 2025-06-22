using CarRentalMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace CarRentalMobile.ViewModels
{
    public partial class MainMenuViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task GoToReservations()
        {
            await Shell.Current.GoToAsync(nameof(ReservationPage));
        }

        [RelayCommand]
        private async Task GoToCities()
        {
            await Shell.Current.GoToAsync(nameof(CitiesPage));
        }
    }
}

