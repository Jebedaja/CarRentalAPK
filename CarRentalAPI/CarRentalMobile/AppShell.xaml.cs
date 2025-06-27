using CarRentalMobile.Views;

namespace CarRentalMobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Rejestracja trasy dla CarsPage
        Routing.RegisterRoute(nameof(CarsPage), typeof(CarsPage));

        // Rejestracja trasy dla nowej strony MyReservationsPage
        Routing.RegisterRoute(nameof(MyReservationsPage), typeof(MyReservationsPage));

        Routing.RegisterRoute(nameof(CitiesPage), typeof(CitiesPage));

        // Jeśli MainDashboardPage jest initial ShellContent, nie musisz jej rejestrować tutaj
        // chyba że chcesz nawigować do niej z innych miejsc poza startem aplikacji.

        // Tutaj możesz rejestrować inne trasy, np. dla strony rezerwacji
        // Routing.RegisterRoute(nameof(ReservationPage), typeof(ReservationPage));
    }
}