using CarRentalMobile.ViewModels; 

namespace CarRentalMobile.Views;

public partial class CitiesPage : ContentPage
{
    // Wstrzykiwanie ViewModelu przez konstruktor
    public CitiesPage(CitiesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // Ustawienie kontekstu wi�zania na ViewModel
    }

    // Metoda wywo�ywana, gdy strona si� pojawi
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Wywo�aj komend� �adowania miast, gdy strona si� pojawi
        if (BindingContext is CitiesViewModel viewModel && viewModel.LoadCitiesCommand.CanExecute(null))
        {
            await viewModel.LoadCitiesCommand.ExecuteAsync(null);
        }
    }
}