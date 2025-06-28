using CarRentalMobile.ViewModels; 

namespace CarRentalMobile.Views;

public partial class MainDashboardPage : ContentPage
{

    public MainDashboardPage(MainDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}