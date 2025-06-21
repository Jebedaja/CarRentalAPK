using CarRentalMobile.Views;

namespace CarRentalMobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Rejestracja trasy dla CarsPage
        Routing.RegisterRoute(nameof(CarsPage), typeof(CarsPage));

        // Tutaj możesz rejestrować inne trasy, np. dla strony rezerwacji
        // Routing.RegisterRoute(nameof(ReservationPage), typeof(ReservationPage));
    }
}