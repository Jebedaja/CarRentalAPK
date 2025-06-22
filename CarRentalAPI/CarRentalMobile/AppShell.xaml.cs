using CarRentalMobile.Views;

namespace CarRentalMobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(MainMenuPage), typeof(MainMenuPage));
        Routing.RegisterRoute(nameof(CitiesPage), typeof(CitiesPage));
        // Rejestracja trasy dla CarsPage
        Routing.RegisterRoute(nameof(CarsPage), typeof(CarsPage));

        Routing.RegisterRoute(nameof(ReservationPage), typeof(ReservationPage));

        Routing.RegisterRoute(nameof(ReservationPage), typeof(ReservationPage));


        // Tutaj możesz rejestrować inne trasy, np. dla strony rezerwacji
        // Routing.RegisterRoute(nameof(ReservationPage), typeof(ReservationPage));
    }
}