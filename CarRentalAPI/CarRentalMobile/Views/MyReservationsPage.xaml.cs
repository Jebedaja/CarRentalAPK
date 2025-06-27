using CarRentalMobile.ViewModels;

namespace CarRentalMobile.Views;

public partial class MyReservationsPage : ContentPage
{
    public MyReservationsPage(MyReservationsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // Ustawienie kontekstu danych
    }

    // Dodaj to zdarzenie, aby wywo�a� metod� ViewModelu po pojawieniu si� strony
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MyReservationsViewModel viewModel)
        {
            viewModel.OnAppearing();
        }
    }
}