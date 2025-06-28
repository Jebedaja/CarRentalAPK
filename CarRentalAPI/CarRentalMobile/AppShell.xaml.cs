using CarRentalMobile.Views;

namespace CarRentalMobile;

public partial class AppShell : Shell
{
    public AppShell()  // podstawowy kontener apki
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(CarsPage), typeof(CarsPage));

        Routing.RegisterRoute(nameof(CitiesPage), typeof(CitiesPage));

        Routing.RegisterRoute(nameof(ReservationDetailsPage), typeof(ReservationDetailsPage));

        Routing.RegisterRoute(nameof(ReservationsPage), typeof(ReservationsPage));

        //mapowanie tras
    }
}