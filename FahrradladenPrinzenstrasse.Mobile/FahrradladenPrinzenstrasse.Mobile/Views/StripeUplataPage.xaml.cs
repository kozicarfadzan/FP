using FahrradladenPrinzenstrasse.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstrasse.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StripeUplataPage : ContentPage
    {
        private readonly Model.Rezervacija rezervacija;

        public StripeUplataPage(Model.Rezervacija rezervacija)
        {
            InitializeComponent();
            var source = new HtmlWebViewSource
            {
                BaseUrl = DependencyService.Get<IBaseUrl>().Get(),
                Html = File.ReadAllText("StripeConfirmPayment.html")
                        .Replace("{APIURL}", APIService.GetApiURL())
                        .Replace("{REZERVACIJA_ID}", rezervacija.RezervacijaId.ToString())
            };

            StripeWebView.Source = source;
            StripeWebView.Navigating += StripeWebView_Navigating;
            this.rezervacija = rezervacija;
        }

        private async void StripeWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if(e.Url.Contains("StripePaymentSuccess.html"))
            {
                if(rezervacija.IsTerminRezervacija || rezervacija.IsServisRezervacija)
                {
                    await Application.Current.MainPage.DisplayAlert("Rezervacija uspješna", "Uspješno ste izvršili rezervaciju.", "OK");
                }
                else if (rezervacija.NacinOtpreme.Cijena == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Narudžba uspješna", "Vaša narudžba je u obradi. Bit ćete obavješteni kada možete preuzeti vašu narudžbu.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Narudžba uspješna", "Vaša narudžba je u obradi. Informacije o dostavi dobit ćete naknadno.", "OK");
                }

                Application.Current.MainPage = new MasterDetailPage();
            }
        }
    }
}