using CarRentalMobile.Views;

namespace CarRentalMobile
{
    public partial class App : Application
    {
        public App(CitiesPage citiesPage)
        {
            InitializeComponent();

            MainPage = new NavigationPage(citiesPage);
        }
    }
}
