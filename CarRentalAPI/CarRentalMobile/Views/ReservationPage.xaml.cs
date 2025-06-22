using System;
using Microsoft.Maui.Controls;

namespace CarRentalMobile.Views
{
    public partial class ReservationPage : ContentPage
    {
        public ReservationPage()
        {
            InitializeComponent();
        }

        private async void OnReserveClicked(object sender, EventArgs e)
        {
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            int.TryParse(AgeEntry.Text, out int age);
            int.TryParse(DaysEntry.Text, out int days);

            await DisplayAlert("Rezerwacja", $"Zarezerwowano na: {firstName} {lastName}, wiek {age}, na {days} dni.", "OK");

            // Tu później dodamy wywołanie metody API do zapisu rezerwacji
        }
    }
}

