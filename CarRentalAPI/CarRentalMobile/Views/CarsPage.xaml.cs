using CarRentalMobile.ViewModels; 

namespace CarRentalMobile.Views;

public partial class CarsPage : ContentPage
{
    public CarsPage(CarsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // Metoda, która zostanie wywo³ana po pojawieniu siê strony
    // Tutaj QueryProperty ju¿ automatycznie ustawi CityId, co wywo³a ³adowanie w ViewModelu
    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    // Jeœli ViewModel ma logikê ³adowania opart¹ na OnAppearing i potrzebuje CityId,
    //    // upewnij siê, ¿e CityId jest ustawione przed wywo³aniem.
    //    // Dziêki [QueryProperty] i OnCityIdChanged w ViewModelu, nie musimy tego robiæ tutaj.
    //}
}