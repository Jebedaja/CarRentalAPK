using CarRentalMobile.ViewModels;

namespace CarRentalMobile.Views;

public partial class ReservationPage : ContentPage
{
    public ReservationPage(ReservationsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
