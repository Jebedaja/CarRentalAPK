using CarRentalMobile.Services;
using CarRentalMobile.ViewModels;
using Microsoft.Maui.Controls;

namespace CarRentalMobile.Views
{
    public partial class ReservationsPage : ContentPage
    {
        public ReservationsPage()
        {
            InitializeComponent();
            var api = new CarRentalApiService();
            BindingContext = new MyReservationsViewModel(api);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as MyReservationsViewModel)?.OnAppearing();
        }
    }
}
