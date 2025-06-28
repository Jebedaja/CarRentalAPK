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

    // gdy strona siê pojawi
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // ³adowanie miast, gdy strona siê pojawi
        if (BindingContext is CitiesViewModel viewModel && viewModel.LoadCitiesCommand.CanExecute(null))
        {
            await viewModel.LoadCitiesCommand.ExecuteAsync(null);
        }
    }
}