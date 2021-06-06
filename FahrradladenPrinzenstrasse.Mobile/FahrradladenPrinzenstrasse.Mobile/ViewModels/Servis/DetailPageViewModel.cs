using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data.Migrations;
using FahrradladenPrinzenstrasse.Mobile.Models;
using FahrradladenPrinzenstrasse.Mobile.Views.DetaljiIznajmiBicikl;
using FahrradladenPrinzenstrasse.Mobile.Views.PotvrdaNarudzbe;
using FahrradladenPrinzenstrasse.Mobile.Views.Termini;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FahrradladenPrinzenstrasse.Mobile.ViewModels.Servis
{
    /// <summary>
    /// ViewModel for detail page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class DetailPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly APIService _serviceServis = new APIService("Servis");

        private readonly double productRating;
        private readonly INavigation Navigation;
        private readonly IList<DateTime> blackoutDates;
        private readonly SfCalendar calendar;

        private Product productDetail;

        private ObservableCollection<Review> reviews;

        private bool isFavourite;

        private bool isReviewVisible;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance for the <see cref="DetailPageViewModel" /> class.
        /// </summary>
        public DetailPageViewModel(Models.Product product, INavigation Navigation, List<DateTime> blackoutDates, SfCalendar calendar)
        {
            this.ProductDetail = product;
            this.Navigation = Navigation;
            this.blackoutDates = blackoutDates;
            this.calendar = calendar;
            if (this.ProductDetail.Reviews == null || this.ProductDetail.Reviews.Count == 0)
            {
                this.IsReviewVisible = true;
            }
            else
            {
                foreach (var review in this.ProductDetail.Reviews)
                {
                    this.productRating += review.Rating;
                }
            }

            if (this.productRating > 0)
                this.ProductDetail.OverallRating = product.OverallRating;

            this.AddFavouriteCommand = new Command(this.AddFavouriteClicked);
            this.BuyNowCommand = new Command(this.BuyNowClicked);
            this.BackButtonCommand = new Command(async () => await BackButtonClicked());

            this.CalendarSelectionChangedCommand = new Command(() => CalendarSelectionChanged());
            //this.CalendarSelectionChangedCommand = new Command(async () => await CalendarSelectionChanged());

            PostaviKalendar();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the property that has been bound with SfRotator and labels, which display the product images and details.
        /// </summary>
        public Product ProductDetail
        {
            get
            {
                return this.productDetail;
            }

            set
            {
                if (this.productDetail == value)
                {
                    return;
                }

                this.productDetail = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with view, which displays the Favourite.
        /// </summary>
        public bool IsFavourite
        {
            get
            {
                return this.isFavourite;
            }
            set
            {
                this.isFavourite = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with view, which displays the empty message.
        /// </summary>
        public bool IsReviewVisible
        {
            get
            {
                if (productDetail.Reviews.Count == 0)
                {
                    this.isReviewVisible = true;
                }

                return this.isReviewVisible;
            }
            set
            {
                this.isReviewVisible = value;
                this.NotifyPropertyChanged();
            }
        }

        private DateTime? _selectedDate;

        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                this._selectedDate = value;
                this.NotifyPropertyChanged();
            }
        }

        private DateTime _minDate;
        public DateTime MinDate
        {
            get { return _minDate; }
            set
            {
                this._minDate = value;
                this.NotifyPropertyChanged();
            }
        }

        private DateTime _maxDate;
        public DateTime MaxDate
        {
            get { return _maxDate; }
            set
            {
                this._maxDate = value;
                this.NotifyPropertyChanged();
            }
        }

        private ObservableCollection<string> _dostupniTermini = new ObservableCollection<string>();

        public ObservableCollection<string> DostupniTermini
        {
            get { return _dostupniTermini; }
            set
            {
                this._dostupniTermini = value;
                this.NotifyPropertyChanged();
            }
        }

        private double _kolicina = 1;

        public double Kolicina
        {
            get { return _kolicina; }
            set
            {
                this._kolicina = value;
                this.NotifyPropertyChanged();
            }
        }

        private string _odabranaSatnica;

        public string OdabranaSatnica
        {
            get { return _odabranaSatnica; }
            set
            {
                this._odabranaSatnica = value;
                this.NotifyPropertyChanged();
            }
        }



        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that will be executed when the Favourite button is clicked.
        /// </summary>
        public Command AddFavouriteCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the AddToCart button is clicked.
        /// </summary>
        public Command BuyNowCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the back button is clicked.
        /// </summary>
        public Command BackButtonCommand { get; set; }
        public Command CalendarSelectionChangedCommand { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Favourite button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void AddFavouriteClicked(object obj)
        {
            if (obj is DetailPageViewModel model)
            {
                model.ProductDetail.IsFavourite = !model.ProductDetail.IsFavourite;
            }
        }

        /// <summary>
        /// Invoked when the Buy Now button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void BuyNowClicked(object obj)
        {
            if (!await ValidateBuyNow())
                return;

            var Satnica = OdabranaSatnica.Split(new char[] { ':' }, 2);
            int SatnicaH = int.Parse(Satnica[0]);
            int SatnicaM = int.Parse(Satnica[1]);

            var Datum = SelectedDate.Value;
            Datum = Datum.AddHours(SatnicaH);
            Datum = Datum.AddMinutes(SatnicaM);

            var request = new Model.Requests.ServisOdaberiTerminRequest
            {
                Id = ProductDetail.Id,
                Kolicina = (int)Kolicina,
                Datum = Datum
            };

            var terminDozvoljen = await _serviceServis.Insert<bool>(request, "OdaberiTermin");
            if(terminDozvoljen == true)
                await Navigation.PushAsync(new ServisAddContactPage(request));
        }

        private async Task<bool> ValidateBuyNow()
        {
            if (SelectedDate == null)
            {
                await Application.Current.MainPage.DisplayAlert("Greška", "Odabir termina je obavezan.", "OK");
                return false;
            }
            if (OdabranaSatnica == null)
            {
                await Application.Current.MainPage.DisplayAlert("Greška", "Odabir satnice je obavezan.", "OK");
                return false;
            }
            if (Kolicina < 1 || Kolicina > 4)
            {
                await Application.Current.MainPage.DisplayAlert("Greška", "Količina mora biti u rasponu od 1 do 4.", "OK");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Invoked when an back button is clicked.
        /// </summary>
        private async Task BackButtonClicked()
        {
            await Navigation.PopAsync();
        }

        public void PostaviKalendar()
        {
            MinDate = DateTime.Today;
            MaxDate = DateTime.Today.AddDays(90); // 3 months
        }

        public async Task LoadBlackoutDates()
        {
            /*var list = await _serviceBicikl.Get<List<DateTime>>(null, "GetDaneBezDostupnihTermina/" + ProductDetail.Id + "/1");
            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    blackoutDates.Add(item);
                }
                calendar.BlackoutDates = blackoutDates;
            }*/
        }


        public async void CalendarSelectionChanged()
        {
            if (SelectedDate != null)
            {
                var request = new Model.Requests.ServisOdaberiTerminRequest
                {
                    Id = ProductDetail.Id,
                    Datum = SelectedDate.Value,
                    Kolicina = (int)Kolicina,
                };

                var termini = await _serviceServis.Insert<List<string>>(request, "DostupniTermini");
                if(termini != null)
                {
                    DostupniTermini.Clear();
                    foreach (var item in termini)
                    {
                        DostupniTermini.Add(item);
                    }

                }
            }

        }



        #endregion
    }
}
