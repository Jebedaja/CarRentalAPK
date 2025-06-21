using CarRentalMobile.ViewModels; 

namespace CarRentalMobile.Views;

public partial class CitiesPage : ContentPage
{
    // Wstrzykiwanie ViewModelu przez konstruktor
    public CitiesPage(CitiesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // Ustawienie kontekstu wi¹zania na ViewModel
    }

    // Metoda wywo³ywana, gdy strona siê pojawi
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Wywo³aj komendê ³adowania miast, gdy strona siê pojawi
        if (BindingContext is CitiesViewModel viewModel && viewModel.LoadCitiesCommand.CanExecute(null))
        {
            await viewModel.LoadCitiesCommand.ExecuteAsync(null);
        }
    }
}