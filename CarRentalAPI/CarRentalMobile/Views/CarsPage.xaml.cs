using CarRentalMobile.ViewModels; 

namespace CarRentalMobile.Views;

public partial class CarsPage : ContentPage
{
    public CarsPage(CarsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // Metoda, kt�ra zostanie wywo�ana po pojawieniu si� strony
    // Tutaj QueryProperty ju� automatycznie ustawi CityId, co wywo�a �adowanie w ViewModelu
    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    // Je�li ViewModel ma logik� �adowania opart� na OnAppearing i potrzebuje CityId,
    //    // upewnij si�, �e CityId jest ustawione przed wywo�aniem.
    //    // Dzi�ki [QueryProperty] i OnCityIdChanged w ViewModelu, nie musimy tego robi� tutaj.
    //}
}