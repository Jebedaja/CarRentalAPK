using CarRentalMobile.ViewModels; // Dodaj tê dyrektywê using

namespace CarRentalMobile.Views;

public partial class MainDashboardPage : ContentPage
{
    // Zmieñ konstruktor, aby przyjmowa³ instancjê MainDashboardViewModel
    public MainDashboardPage(MainDashboardViewModel viewModel)
    {
        InitializeComponent();
        // Ustaw BindingContext strony na wstrzykniêty ViewModel
        BindingContext = viewModel;
    }
}