using CarRentalMobile.ViewModels; // Dodaj t� dyrektyw� using

namespace CarRentalMobile.Views;

public partial class MainDashboardPage : ContentPage
{
    // Zmie� konstruktor, aby przyjmowa� instancj� MainDashboardViewModel
    public MainDashboardPage(MainDashboardViewModel viewModel)
    {
        InitializeComponent();
        // Ustaw BindingContext strony na wstrzykni�ty ViewModel
        BindingContext = viewModel;
    }
}