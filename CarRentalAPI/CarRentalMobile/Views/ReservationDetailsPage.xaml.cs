namespace CarRentalMobile.Views;
using CarRentalMobile.ViewModels;

public partial class ReservationDetailsPage : ContentPage
{
    public ReservationDetailsPage(ReservationDetailsViewModel viewModel) 
    {
        InitializeComponent();
        BindingContext = viewModel; 
    }
}