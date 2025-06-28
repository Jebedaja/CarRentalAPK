using CarRentalMobile.ViewModels;
using CarRentalMobile.Views;

namespace CarRentalMobile.Views
{
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
            BindingContext = new MainMenuViewModel(); // wstrzykiwanie ViewModelu
        }

        private async void OnReservationClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CitiesPage)); // nawigacja do listy miast
        }

        private async void OnMyReservationsClicked(object sender, EventArgs e)
        {
            // to juz nie istnieje?
            await Shell.Current.DisplayAlert("Info", "Widok Moje Rezerwacje jeszcze nie istnieje.", "OK");
        }
    }
}
