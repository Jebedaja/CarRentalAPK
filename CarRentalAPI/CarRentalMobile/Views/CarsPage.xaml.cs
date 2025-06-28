using CarRentalMobile.ViewModels; 

namespace CarRentalMobile.Views;

public partial class CarsPage : ContentPage
{
    public CarsPage(CarsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}