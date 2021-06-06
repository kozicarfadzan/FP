using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstrasse.Mobile
{
    public partial class App : Application
    {
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        private readonly APIService _serviceKorisnik = new APIService("Korisnik");
        public static string BaseBiciklImageUrl { get; } = "http://localhost:58794/api/Bicikl/Photo/";
        public App()
        {
            InitializeComponent();

        }

        protected async override void OnStart()
        {
            string errorMessage = null;
            try
            {
                string username = await SecureStorage.GetAsync("username");
                string password = await SecureStorage.GetAsync("password");

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    APIService.Username = username;
                    APIService.Password = password;

                    try
                    {
                        APIService.CurrentUser = await _serviceKorisnik.Get<Model.Korisnik>(null, "MyProfile");
                        if(APIService.CurrentUser != null)
                        {
                            if ((await APIService.GetCurrentUser()).Uloga == "Klijent")
                            {
                                Current.MainPage = new MasterDetailPage();
                                return;
                            }
                            else
                                errorMessage = "Nemate pravo pristupa";
                        }

                    }
                    catch (Exception)
                    {
                        errorMessage = "Sesija je istekla, molimo da se prijavite ponovo.";
                    }
                }

            }
            catch (Exception)
            {
                // Possible that device doesn't support secure storage on device.
            }

            MainPage = new Views.Login.SimpleLoginPage();
            if(errorMessage != null)
                await MainPage.DisplayAlert("Error", errorMessage, "OK");

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
