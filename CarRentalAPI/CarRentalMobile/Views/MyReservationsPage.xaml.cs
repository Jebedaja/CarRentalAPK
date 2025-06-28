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

            var apiService = new CarRentalApiService(); // inicjalizacja serwisu api
            viewModel = new MyReservationsViewModel(apiService); //

            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing(); 
            viewModel.OnAppearing(); // ladowanie rezerwacji przy pojawieniu siê strony
        }
    }
}
