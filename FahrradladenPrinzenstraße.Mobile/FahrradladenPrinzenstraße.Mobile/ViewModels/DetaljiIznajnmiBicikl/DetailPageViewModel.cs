﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Mobile.Models;
using FahrradladenPrinzenstraße.Mobile.Views.DetaljiIznajmiBicikl;
using FahrradladenPrinzenstraße.Mobile.Views.Termini;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FahrradladenPrinzenstraße.Mobile.ViewModels.DetaljiIznajmiBicikl
{
    /// <summary>
    /// ViewModel for detail page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class DetailPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly APIService _serviceBicikl = new APIService("Bicikl");
        private readonly APIService _serviceTerminStavka = new APIService("TerminStavka");

        private readonly double productRating;
        private readonly INavigation Navigation;
        private readonly IList<DateTime> blackoutDates;
        private readonly SfCalendar calendar;

        private Product productDetail;


        private ObservableCollection<Category> categories;

        private ObservableCollection<Review> reviews;

        private bool isFavourite;

        private bool isReviewVisible;

        private int? cartItemCount;

        private int _selectedSizeIndex;

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

            for (int i = 0; i < ProductDetail.SizeVariants.Count; i++)
            {
                if (ProductDetail.SizeVariants[i] == ProductDetail.Bicikl.VelicinaOkvira.Naziv)
                {
                    SelectedSizeIndex = i;
                    break;
                }
            }

            this.AddFavouriteCommand = new Command(this.AddFavouriteClicked);
            this.BuyNowCommand = new Command(this.BuyNowClicked);
            this.AddToCartCommand = new Command(this.AddToCartClicked);
            this.ShareCommand = new Command(this.ShareClicked);
            this.VariantCommand = new Command(async () => await this.VariantClicked());
            this.ItemSelectedCommand = new Command(this.ItemSelected);
            this.CartItemCommand = new Command(this.CartClicked);
            this.LoadMoreCommand = new Command(this.LoadMoreClicked);
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
        /// Gets or sets the property that has been bound with SfButtons, which represents the index of the selected product size
        /// </summary>
        public int SelectedSizeIndex
        {
            get
            {
                return this._selectedSizeIndex;
            }

            set
            {
                if (this._selectedSizeIndex == value)
                {
                    return;
                }

                this._selectedSizeIndex = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with StackLayout, which displays the categories using ComboBox.
        /// </summary>
        public ObservableCollection<Category> Categories
        {
            get
            {
                return this.categories;
            }

            set
            {
                if (this.categories == value)
                {
                    return;
                }

                this.categories = value;
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

        /// <summary>
        /// Gets or sets the property that has been bound with view, which displays the cart items count.
        /// </summary>
        public int? CartItemCount
        {
            get
            {
                return this.cartItemCount;
            }
            set
            {
                this.cartItemCount = value;
                this.NotifyPropertyChanged();
            }
        }


        /*private List<DateTime> _blackoutDates;

        public List<DateTime> BlackoutDates
        {
            get { return _blackoutDates; }
            set
            {
                this._blackoutDates = value;
                this.NotifyPropertyChanged();
            }
        }*/

        private SelectionRange _selectedRange;

        public SelectionRange SelectedRange
        {
            get { return _selectedRange; }
            set
            {
                this._selectedRange = value;
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

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that will be executed when the Favourite button is clicked.
        /// </summary>
        public Command AddFavouriteCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the AddToCart button is clicked.
        /// </summary>
        public Command AddToCartCommand { get; set; }
        public Command BuyNowCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the Share button is clicked.
        /// </summary>
        public Command ShareCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the size button is clicked.
        /// </summary>
        public Command VariantCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when cart button is clicked.
        /// </summary>
        public Command CartItemCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the Show All button is clicked.
        /// </summary>
        public Command LoadMoreCommand { get; set; }

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
        /// Invoked when the Cart button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void AddToCartClicked(object obj)
        {
            if (SelectedRange == null || SelectedRange.StartDate == null || SelectedRange.EndDate == null)
            {
                await Application.Current.MainPage.DisplayAlert("Greška", "Odabir termina je obavezan.", "OK");
                return;
            }

            this.cartItemCount = this.cartItemCount ?? 0;
            this.CartItemCount += 1;

            var request = new Model.Requests.BiciklOdaberiTerminRequest
            {
                Id = ProductDetail.Id,
                Kolicina = 1,
                DatumPreuzimanja = SelectedRange.StartDate,
                DatumVracanja = SelectedRange.EndDate
            };
            try
            {
                var stavka = await _serviceBicikl.Insert<Model.TerminStavka>(request, "OdaberiTermin");
                if (stavka != null)
                    await Navigation.PushAsync(new CartPage());
                else
                    await UpdateCartItemCount();
            }
            catch (Exception e)
            {

            }
        }

        public async Task UpdateCartItemCount()
        {
            var count = (await _serviceTerminStavka.Get<int>(null, "GetBrojStavki"));
            if (count > 0 || CartItemCount != null)
            {
                CartItemCount = count;
            }

        }

        /// <summary>
        /// Invoked when the Buy Now button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void BuyNowClicked(object obj)
        {
            if (SelectedRange == null || SelectedRange.StartDate == null || SelectedRange.EndDate == null)
            {
                await Application.Current.MainPage.DisplayAlert("Greška", "Odabir termina je obavezan.", "OK");
                return;
            }

            this.cartItemCount = this.cartItemCount ?? 0;
            this.CartItemCount += 1;

            var request = new Model.Requests.BiciklOdaberiTerminRequest
            {
                Id = ProductDetail.Id,
                Kolicina = 1,
                DatumPreuzimanja = SelectedRange.StartDate,
                DatumVracanja = SelectedRange.EndDate
            };

            var stavka = await _serviceBicikl.Insert<bool>(request, "OdaberiTermin");
            if (stavka == true)
                await Navigation.PushAsync(new CartPage());
            else
                await UpdateCartItemCount();


        }

        /// <summary>
        /// Invoked when the Share button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void ShareClicked(object obj)
        {
            // Do something.
        }

        /// <summary>
        /// Invoked when the variant button is clicked.
        /// </summary>
        private async Task VariantClicked()
        {
            if (SelectedSizeIndex < 0 || SelectedSizeIndex >= ProductDetail.SizeVariants.Count)
            {
                await Application.Current.MainPage.DisplayAlert("Greška", "Neispravna veličina je odabrana", "OK");
                return;
            }

            if (ProductDetail.SizeVariants[SelectedSizeIndex] == ProductDetail.Bicikl.VelicinaOkvira.Naziv)
                return;

            var request = new Model.Requests.BiciklSearchRequest
            {
                ModelId = ProductDetail.Bicikl.ModelId,
                VelicinaOkvira = ProductDetail.SizeVariants[SelectedSizeIndex],
                Stanje = new List<Model.Stanje> { ProductDetail.Stanje }
            };
            var list = await _serviceBicikl.Get<List<Model.Bicikl>>(request);
            if (list.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Greška", "Nije pronađeno biciklo sa odabranom veličinom okvira", "OK");
                return;
            }

            var similarity = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                similarity.Add(0);
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].BojaId == productDetail.Bicikl.BojaId)
                    similarity[i]++;
                if (list[i].GodinaProizvodnje == productDetail.Bicikl.GodinaProizvodnje)
                    similarity[i]++;
                if (list[i].NoznaKocnica == productDetail.Bicikl.NoznaKocnica)
                    similarity[i]++;
                if (list[i].Stanje == productDetail.Bicikl.Stanje)
                    similarity[i]++;
                if (list[i].StarosnaGrupaId == productDetail.Bicikl.StarosnaGrupaId)
                    similarity[i]++;
            }

            int najslicnijiIndex = similarity.IndexOf(similarity.Max());
            var item = list[najslicnijiIndex];

            await Navigation.PushAsync(new DetailPage(
               new Product
               {
                   Id = item.BiciklId,
                   ActualPrice = item.Stanje == Model.Stanje.Korišteno ? item.CijenaPoDanu.Value : item.Cijena.Value,
                   Name = item.PuniNaziv,
                   SizeVariants = item.SizeVariants,
                   Stanje = item.Stanje,
                   Slika = item.Slika,
                   Description = item.Opis,
                   OverallRating = item.Ocjena,
                   Bicikl = item
               })
             );
        }

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        private void ItemSelected(object attachedObject)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when cart icon button is clicked.
        /// </summary>
        private void CartClicked()
        {
            Navigation.PushAsync(new CartPage());
        }

        /// <summary>
        /// Invoked when Load more button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void LoadMoreClicked(object obj)
        {
            // Do something
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
            var list = await _serviceBicikl.Get<List<DateTime>>(null, "GetDaneBezDostupnihTermina/" + ProductDetail.Id + "/1");
            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    blackoutDates.Add(item);
                }
                calendar.BlackoutDates = blackoutDates;
            }

        }


        //private Task CalendarSelectionChanged()
        private void CalendarSelectionChanged()
        {
            if (SelectedRange != null && SelectedRange.StartDate != null)
            {
                if (SelectedRange.EndDate != null)
                {
                    // todo api call za duplu provjeru 
                }
            }
            else
            {
            }


        }



        #endregion
    }
}
