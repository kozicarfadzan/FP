using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FahrradladenPrinzenstrasse.Mobile.ViewModels.Signup
{
    /// <summary>
    /// ViewModel for sign-up page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SignUpPageViewModel : LoginViewModel
    {
        private readonly APIService _serviceKorisnik = new APIService("Korisnik");
        private readonly APIService _serviceGrad = new APIService("Grad");
        private readonly INavigation navigation;
        #region Fields

        private string ime;
        private string prezime;
        private string korisnickoIme;
        private string lozinka;
        private string lozinkaPotvrda;
        private string adresaStanovanja;
        private string brojTelefona;
        private Model.Grad odabraniGrad;
        private string odabraniSpol;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="SignUpPageViewModel" /> class.
        /// </summary>
        public SignUpPageViewModel(INavigation navigation)
        {
            this.LoginCommand = new Command(() => this.LoginClicked());
            this.SignUpCommand = new Command(async () => await this.SignUpClicked());

            Spolovi.Add("Muski");
            Spolovi.Add("Zenski");

            this.navigation = navigation;
        }

        public async Task Init()
        {
            Gradovi.Clear();
            var list = await _serviceGrad.Get<List<Model.Grad>>(null);
            foreach (var item in list)
            {
                Gradovi.Add(item);
            }
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the name from user in the Sign Up page.
        /// </summary>
        public string Ime
        {
            get
            {
                return this.ime;
            }

            set
            {
                if (this.ime == value)
                {
                    return;
                }

                this.ime = value;
                this.NotifyPropertyChanged();
            }
        }
        public string Prezime
        {
            get
            {
                return this.prezime;
            }

            set
            {
                if (this.prezime == value)
                {
                    return;
                }

                this.prezime = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password from users in the Sign Up page.
        /// </summary>
        public string Lozinka
        {
            get
            {
                return this.lozinka;
            }

            set
            {
                if (this.lozinka == value)
                {
                    return;
                }

                this.lozinka = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password confirmation from users in the Sign Up page.
        /// </summary>
        public string LozinkaPotvrda
        {
            get
            {
                return this.lozinkaPotvrda;
            }

            set
            {
                if (this.lozinkaPotvrda == value)
                {
                    return;
                }

                this.lozinkaPotvrda = value;
                this.NotifyPropertyChanged();
            }
        }
        public string KorisnickoIme
        {
            get
            {
                return this.korisnickoIme;
            }

            set
            {
                if (this.korisnickoIme == value)
                {
                    return;
                }

                this.korisnickoIme = value;
                this.NotifyPropertyChanged();
            }
        }
        public string AdresaStanovanja
        {
            get
            {
                return this.adresaStanovanja;
            }

            set
            {
                if (this.adresaStanovanja == value)
                {
                    return;
                }

                this.adresaStanovanja = value;
                this.NotifyPropertyChanged();
            }
        }
        
        public string BrojTelefona
        {
            get
            {
                return this.brojTelefona;
            }

            set
            {
                if (this.brojTelefona == value)
                {
                    return;
                }

                this.brojTelefona = value;
                this.NotifyPropertyChanged();
            }
        }
        
        public Model.Grad OdabraniGrad
        {
            get
            {
                return this.odabraniGrad;
            }

            set
            {
                if (this.odabraniGrad == value)
                {
                    return;
                }

                this.odabraniGrad = value;
                this.NotifyPropertyChanged();
            }
        }

        public string OdabraniSpol
        {
            get
            {
                return this.odabraniSpol;
            }

            set
            {
                if (this.odabraniSpol == value)
                {
                    return;
                }

                this.odabraniSpol = value;
                this.NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Model.Grad> Gradovi { get; set; } = new ObservableCollection<Model.Grad>();
        public ObservableCollection<string> Spolovi { get; set; } = new ObservableCollection<string>();

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Log in button is clicked.
        /// </summary>
        private void LoginClicked()
        {
            Application.Current.MainPage = (new Views.Login.SimpleLoginPage());
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        private async Task SignUpClicked()
        {
            if(OdabraniSpol == null)
            {
                await Application.Current.MainPage.DisplayAlert("Greška", "Odabir spola je obavezan.", "OK");
                return;
            }
            if(OdabraniGrad == null)
            {
                await Application.Current.MainPage.DisplayAlert("Greška", "Odabir grada je obavezan.", "OK");
                return;
            }

            var request = new Model.Requests.KorisnikInsertRequest {
                AdresaStanovanja = AdresaStanovanja,
                BrojTelefona = BrojTelefona,
                Email = Email,
                GradID = OdabraniGrad.GradID,
                Ime = Ime,
                Prezime = Prezime,
                KorisnickoIme = KorisnickoIme,
                Lozinka = Lozinka,
                LozinkaPotvrda = LozinkaPotvrda,
                Spol = OdabraniSpol.Substring(0, 1)
            };
            var korisnik = await _serviceKorisnik.Insert<Model.Korisnik>(request);
            if(korisnik != null)
            {
                APIService.Username = KorisnickoIme;
                APIService.Password = Lozinka;

                APIService.CurrentUser = await _serviceKorisnik.Get<Model.Korisnik>(null, "MyProfile");

                await SecureStorage.SetAsync("username", KorisnickoIme);
                await SecureStorage.SetAsync("password", Lozinka);

                Application.Current.MainPage = new MasterDetailPage();
            }
        }

        #endregion
    }
}