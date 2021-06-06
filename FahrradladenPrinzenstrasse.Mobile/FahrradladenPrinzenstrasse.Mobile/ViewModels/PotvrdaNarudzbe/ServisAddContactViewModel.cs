using AutoMapper;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Mobile.Models.PotvrdaNarudzbe;
using FahrradladenPrinzenstrasse.Mobile.Views;
using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FahrradladenPrinzenstrasse.Mobile.ViewModels.PotvrdaNarudzbe
{
    /// <summary>
    /// ViewModel for add contact page.
    /// </summary>
    public class ServisAddContactViewModel : LoginViewModel
    {
        #region Fields
        private readonly APIService _serviceKorisnik = new APIService("Korisnik");
        private readonly APIService _serviceRezervacija = new APIService("Rezervacija");
        private readonly INavigation Navigation;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance for the <see cref="AddContactViewModel" /> class.
        /// </summary>
        public ServisAddContactViewModel(INavigation navigation, Model.Requests.ServisOdaberiTerminRequest request)
        {
            NaciniPlacanjaListHeight = NaciniPlacanja.Count * 112;

            this.SubmitCommand = new Command(this.SubmitButtonClicked);
            this.InitCommand = new Command(async () => await this.Init());
            InitCommand.Execute(null);
            this.Navigation = navigation;
            Request = request;

            DetaljiListViewHeight = Request.Kolicina * 265;

            var TipoviList = new ObservableCollection<string>();
            foreach (Model.Tip item in Enum.GetValues(typeof(Model.Tip)))
            {
                TipoviList.Add(item.ToString());
            }

            for (int i = 0; i < Request.Kolicina; i++)
            {
                DetaljiServisa.Add(new DetaljiServisaMobile()
                {
                    DetaljiText = "Detalji bicikla " + (i + 1) + " za servis",
                    TipoviBicikala = TipoviList
                });
            }

        }

        private async Task Init()
        {
            Korisnik = await _serviceKorisnik.Get<Model.Korisnik>(null, "MyProfile");
        }

        #endregion

        #region Property

        private NacinPlacanja _odabraniNacinPlacanja;

        public NacinPlacanja OdabraniNacinPlacanja
        {
            get { return _odabraniNacinPlacanja; }
            set
            {
                if (this._odabraniNacinPlacanja == value)
                {
                    return;
                }

                this._odabraniNacinPlacanja = value;
                this.NotifyPropertyChanged();

            }
        }

        public ObservableCollection<NacinPlacanja> NaciniPlacanja { get; set; } = new ObservableCollection<NacinPlacanja>
        {
            new NacinPlacanja
            {
                Nacin = "online",
                Opis = "Online plaćanje putem kartice",
                Slika = File.ReadAllBytes("credit_card.png")
            }
        };

        private int _naciniPlacanjaListHeight;

        public int NaciniPlacanjaListHeight
        {
            get { return _naciniPlacanjaListHeight; }
            set
            {
                if (this._naciniPlacanjaListHeight == value)
                {
                    return;
                }

                this._naciniPlacanjaListHeight = value;
                this.NotifyPropertyChanged();
            }
        }

        private double _detaljiListViewHeight;

        public double DetaljiListViewHeight
        {
            get { return _detaljiListViewHeight; }
            set
            {
                if (this._detaljiListViewHeight == value)
                {
                    return;
                }

                this._detaljiListViewHeight = value;
                this.NotifyPropertyChanged();
            }
        }

        public ObservableCollection<DetaljiServisaMobile> DetaljiServisa { get; set; } = new ObservableCollection<DetaljiServisaMobile>();

        private Model.Tip _tip;

        public Model.Tip Tip
        {
            get { return _tip; }
            set
            {
                if (this._tip == value)
                {
                    return;
                }

                this._tip = value;
                this.NotifyPropertyChanged();
            }
        }
        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that will be executed when the submit button is clicked.
        /// </summary>
        public Command SubmitCommand { get; set; }
        public Command InitCommand { get; set; }
        public ServisOdaberiTerminRequest Request { get; }
        public Model.Korisnik Korisnik { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the submit button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SubmitButtonClicked(object obj)
        {
            if (Korisnik == null)
                return;

            if(OdabraniNacinPlacanja == null)
                OdabraniNacinPlacanja = NaciniPlacanja[0];

            if (!ValidateFields(out string error))
            {
                await Application.Current.MainPage.DisplayAlert("Greška", error, "OK");
                return;
            }

            var request = new Model.Requests.RezervacijaInsertRequest
            {
                Ime = Korisnik.Ime,
                Prezime = Korisnik.Prezime,
                Email = Korisnik.Email,
                AdresaStanovanja = Korisnik.AdresaStanovanja,
                BrojTelefona = Korisnik.BrojTelefona,
                Grad = Korisnik.Grad.Naziv,
                NacinPlacanja = OdabraniNacinPlacanja.Nacin,
                RezervacijaServis = new List<Model.RezervacijaServis>(),
                
            };

            foreach (var item in DetaljiServisa)
            {
                var stavka = new Model.RezervacijaServis
                {
                    DatumServisiranja = Request.Datum,
                    ServisId = Request.Id,
                    Boja = item.Boja,
                    Model = item.Model,
                    DodatniTroskovi = int.TryParse(item.DodatniTroskovi, out int troskovi) ? troskovi : 0,
                    Opis = item.Opis,
                    Proizvodjac = item.Proizvodjac
                };
                if (Enum.TryParse(item.Tip, out Model.Tip result))
                    stavka.Tip = result;

                request.RezervacijaServis.Add(stavka);
            }

            var Rezervacija = await _serviceRezervacija.Insert<Model.Rezervacija>(request);
            if (Rezervacija != null)
            {
                Rezervacija.IsServisRezervacija = true;
                if (OdabraniNacinPlacanja.Nacin == "online")
                {
                    await Navigation.PushAsync(new StripeUplataPage(Rezervacija));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Narudžba uspješna", "Vaša narudžba je u obradi. Bit ćete obavješteni kada možete preuzeti vašu narudžbu.", "OK");

                    Application.Current.MainPage = new MasterDetailPage();
                }
            }


        }

        private bool ValidateFields(out string error)
        {
            //if (string.IsNullOrWhiteSpace(Korisnik.Ime))
            //{
            //    error = "Polje Ime je obavezno.";
            //}
            //else if (string.IsNullOrWhiteSpace(Korisnik.Prezime))
            //{
            //    error = "Polje Prezime je obavezno.";
            //}
            //else if (string.IsNullOrWhiteSpace(Korisnik.AdresaStanovanja))
            //{
            //    error = "Polje Adresa je obavezno.";
            //}
            //else if (string.IsNullOrWhiteSpace(Korisnik.Email))
            //{
            //    error = "Polje Email je obavezno.";
            //}
            //else if (string.IsNullOrWhiteSpace(Korisnik.Grad.Naziv))
            //{
            //    error = "Polje Grad je obavezno.";
            //}
            //else if (OdabraniNacinPlacanja == null)
            //{
            //    error = "Odaberite način plaćanja.";
            //}
            //else
            //{
            error = null;
            return true;
            //}
            //return false;
        }

        #endregion

    }
}
