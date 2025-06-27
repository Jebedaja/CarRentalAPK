using CarRentalMobile.Services;
using CarRentalMobile.ViewModels;
using Microsoft.Maui.Controls;

namespace CarRentalMobile.Views
{
    public partial class MyReservationsPage : ContentPage
    {
        private readonly MyReservationsViewModel viewModel;

        public MyReservationsPage()
        {
            InitializeComponent();

            // Tworzymy serwis i ViewModel
            var apiService = new CarRentalApiService();
            viewModel = new MyReservationsViewModel(apiService);

            // Przypisujemy kontekst danych
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Wywo³ujemy za³adowanie rezerwacji
            viewModel.OnAppearing();
        }
    }
}
