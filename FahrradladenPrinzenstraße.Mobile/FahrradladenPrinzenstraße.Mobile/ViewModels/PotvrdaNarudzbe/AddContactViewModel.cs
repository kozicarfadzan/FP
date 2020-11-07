using AutoMapper;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Mobile.Models.PotvrdaNarudzbe;
using FahrradladenPrinzenstraße.Mobile.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FahrradladenPrinzenstraße.Mobile.ViewModels.PotvrdaNarudzbe
{
    /// <summary>
    /// ViewModel for add contact page.
    /// </summary>
    public class AddContactViewModel : LoginViewModel
    {
        #region Fields
        private readonly APIService _serviceKorisnik = new APIService("Korisnik");
        private readonly APIService _serviceNacinOtpreme = new APIService("NacinOtpreme");
        private readonly APIService _serviceRezervacija = new APIService("Rezervacija");
        private readonly INavigation Navigation;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance for the <see cref="AddContactViewModel" /> class.
        /// </summary>
        public AddContactViewModel(INavigation navigation)
        {
            NaciniPlacanjaListHeight = NaciniPlacanja.Count * 112;
            this.SubmitCommand = new Command(this.SubmitButtonClicked);
            this.InitCommand = new Command(async () => await this.Init());
            InitCommand.Execute(null);
            this.Navigation = navigation;
        }

        private async Task Init()
        {
            Korisnik = await _serviceKorisnik.Get<Model.Korisnik>(null, "MyProfile");
            var ListNaciniOtpreme = await _serviceNacinOtpreme.Get<List<Model.NacinOtpreme>>(null);
            NaciniOtpreme.Clear();
            foreach (var item in ListNaciniOtpreme)
            {
                NaciniOtpreme.Add(item);
            }
            NaciniOtpremeListHeight = NaciniOtpreme.Count * 112;
        }

        #endregion

        #region Property

        private Model.Korisnik _korisnik;
        public Model.Korisnik Korisnik
        {
            get { return _korisnik; }
            set
            {
                if (this._korisnik == value)
                {
                    return;
                }

                this._korisnik = value;
                this.NotifyPropertyChanged();
            }
        }


        private string _postanskiKod;
        public string PostanskiKod
        {
            get { return _postanskiKod; }
            set
            {
                if (this._postanskiKod == value)
                {
                    return;
                }

                this._postanskiKod = value;
                this.NotifyPropertyChanged();
            }
        }


        private string _pokrajina;
        public string Pokrajina
        {
            get { return _pokrajina; }
            set
            {
                if (this._pokrajina == value)
                {
                    return;
                }

                this._pokrajina = value;
                this.NotifyPropertyChanged();
            }
        }

        private string _drzava;
        public string Drzava
        {
            get { return _drzava; }
            set
            {
                if (this._drzava == value)
                {
                    return;
                }

                this._drzava = value;
                this.NotifyPropertyChanged();
            }
        }

        private Model.NacinOtpreme _odabraniNacinOtpreme;

        public Model.NacinOtpreme OdabraniNacinOtpreme
        {
            get { return _odabraniNacinOtpreme; }
            set
            {
                if (this._odabraniNacinOtpreme == value)
                {
                    return;
                }

                this._odabraniNacinOtpreme = value;
                this.NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Model.NacinOtpreme> NaciniOtpreme { get; set; } = new ObservableCollection<Model.NacinOtpreme>();

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
                Nacin = "onpickup",
                Opis = "Plaćanje pri preuzimanju / Plaćanje pouzećem",
                Slika = File.ReadAllBytes("cash_on_delivery.png")
            },
            new NacinPlacanja
            {
                Nacin = "online",
                Opis = "Online plaćanje putem kartice",
                Slika = File.ReadAllBytes("credit_card.png")
            }
        };

        private int _naciniOtpremeListHeight;

        public int NaciniOtpremeListHeight
        {
            get { return _naciniOtpremeListHeight; }
            set
            {
                if (this._naciniOtpremeListHeight == value)
                {
                    return;
                }

                this._naciniOtpremeListHeight = value;
                this.NotifyPropertyChanged();
            }
        }

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



        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that will be executed when the submit button is clicked.
        /// </summary>
        public Command SubmitCommand { get; set; }
        public Command InitCommand { get; set; }

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
                Država = Drzava,
                Grad = Korisnik.Grad.Naziv,
                Pokrajina = Pokrajina,
                PostanskiKod = PostanskiKod,
                NacinOtpremeId = OdabraniNacinOtpreme.NacinOtpremeId,
                NacinPlacanja = OdabraniNacinPlacanja.Nacin
            };

            var Rezervacija = await _serviceRezervacija.Insert<Model.Rezervacija>(request);
            if (Rezervacija != null)
            {
                if (OdabraniNacinPlacanja.Nacin == "online")
                {
                    await Navigation.PushAsync(new StripeUplataPage(Rezervacija));
                }
                else
                {
                    if (OdabraniNacinOtpreme.Cijena == 0)
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

        private bool ValidateFields(out string error)
        {
            if (string.IsNullOrWhiteSpace(Korisnik.Ime))
            {
                error = "Polje Ime je obavezno.";
            }
            else if (string.IsNullOrWhiteSpace(Korisnik.Prezime))
            {
                error = "Polje Prezime je obavezno.";
            }
            else if (string.IsNullOrWhiteSpace(Korisnik.AdresaStanovanja))
            {
                error = "Polje Adresa je obavezno.";
            }
            else if (string.IsNullOrWhiteSpace(Korisnik.Email))
            {
                error = "Polje Email je obavezno.";
            }
            else if (string.IsNullOrWhiteSpace(Korisnik.Grad.Naziv))
            {
                error = "Polje Grad je obavezno.";
            }
            else if (string.IsNullOrWhiteSpace(Pokrajina))
            {
                error = "Polje Pokrajina je obavezno.";
            }
            else if (string.IsNullOrWhiteSpace(Drzava))
            {
                error = "Polje Država je obavezno.";
            }
            else if (string.IsNullOrWhiteSpace(PostanskiKod))
            {
                error = "Polje Poštanski Kod je obavezno.";
            }
            else if (OdabraniNacinOtpreme == null)
            {
                error = "Odaberite način otpreme.";
            }
            else if (OdabraniNacinPlacanja == null)
            {
                error = "Odaberite način plaćanja.";
            }
            else
            {
                error = null;
                return true;
            }
            return false;
        }

        #endregion

    }
}
