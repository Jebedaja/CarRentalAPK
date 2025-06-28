using CarRentalMobile.ViewModels; 

namespace CarRentalMobile.Views;

public partial class CitiesPage : ContentPage
{
    // Wstrzykiwanie ViewModelu 
    public CitiesPage(CitiesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; 
    }

    // gdy strona si� pojawi
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // �adowanie miast, gdy strona si� pojawi
        if (BindingContext is CitiesViewModel viewModel && viewModel.LoadCitiesCommand.CanExecute(null))
        {
            await viewModel.LoadCitiesCommand.ExecuteAsync(null);
        }
    }
}