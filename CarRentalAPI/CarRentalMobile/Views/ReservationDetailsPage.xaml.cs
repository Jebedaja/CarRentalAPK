namespace CarRentalMobile.Views;
using CarRentalMobile.ViewModels;

public partial class ReservationDetailsPage : ContentPage
{
    public ReservationDetailsPage(ReservationDetailsViewModel viewModel) // DI wstrzykuje ViewModel
    {
        InitializeComponent();
        BindingContext = viewModel; // To jest kluczowe!
    }
}