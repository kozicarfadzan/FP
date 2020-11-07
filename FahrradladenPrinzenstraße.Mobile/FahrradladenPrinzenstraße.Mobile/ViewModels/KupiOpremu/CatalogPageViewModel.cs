using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Mobile.Controls;
using FahrradladenPrinzenstraße.Mobile.Models;
using FahrradladenPrinzenstraße.Mobile.Views.Korpa;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;

namespace FahrradladenPrinzenstraße.Mobile.ViewModels.KupiOpremu
{
    /// <summary>
    /// ViewModel for catalog page.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    [DataContract]
    public class CatalogPageViewModel : BaseViewModel
    {
        private readonly APIService _serviceOprema = new APIService("Oprema");
        private readonly APIService _serviceKorpaStavka = new APIService("KorpaStavka");
        private readonly APIService _serviceProizvodjac = new APIService("Proizvodjac");
        private readonly INavigation Navigation;

        #region Fields

        private List<Category> filterOptions;

        private List<string> sortOptions;


        private Model.Requests.OpremaSearchRequest request = new Model.Requests.OpremaSearchRequest();

        private Command addFavouriteCommand;

        private Command itemSelectedCommand;

        private Command sortCommand;

        private Command filterCommand;

        private Command addToCartCommand;

        private Command cartItemCommand;

        private string cartItemCount;
        private bool filteriUcitani;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="CatalogPageViewModel" /> class.
        /// </summary>
        public CatalogPageViewModel(INavigation navigation)
        {
            this.FilterOptions = new List<Category>();

            this.SortOptions = new List<string>
            {
                "New Arrivals",
                "Price - high to low",
                "Price - Low to High",
                "Popularity",
                "Discount"
            };
            this.Navigation = navigation;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the property that has been bound with a list view, which displays the item details in tile.
        /// </summary>
        public ObservableCollection<Product> Products
        {
            get; set;
        } = new ObservableCollection<Product>();

        /// <summary>
        /// Gets or sets the property that has been bound with a list view, which displays the filter options.
        /// </summary>
        public List<Category> FilterOptions
        {
            get
            {
                return this.filterOptions;
            }

            set
            {
                if (this.filterOptions == value)
                {
                    return;
                }

                this.filterOptions = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property has been bound with a list view, which displays the sort details.
        /// </summary>
        public List<string> SortOptions
        {
            get
            {
                return this.sortOptions;
            }

            set
            {
                if (this.sortOptions == value)
                {
                    return;
                }

                this.sortOptions = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with view, which displays the cart items count.
        /// </summary>
        public string CartItemCount
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

        public async Task UcitajOpremu()
        {
            var list = await _serviceOprema.Get<List<Model.Oprema>>(request);
            Products.Clear();

            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    Products.Add(new Product
                    {
                        Id = item.OpremaId,
                        ActualPrice = item.Cijena,
                        Name = item.Naziv,
                        Slika = item.Slika,
                        Description = item.Opis,
                        Oprema = item,
                        OverallRating = item.Ocjena
                    });
                }
            }
            else
            {
                //Products.Add(new Model.Bicikl
                //{
                //    ResultText = "No results found."
                //});
            }
        }


        public async Task UcitajFiltere()
        {
            this.filterOptions.Clear();
        

            var proizvodjaci = await _serviceProizvodjac.Get<List<Model.Proizvodjac>>(null);
            this.filterOptions.Add(
                new Category
                {
                    Name = "Proizvođač",
                    FieldName = "ProizvodjacId",
                    SubCategories = proizvodjaci
                       .ToDictionary(t => (int)t.ProizvodjacId, t => t.Naziv),
                    FilterType = "checkbox"
                }
            );
           
            

            filteriUcitani = true;
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        //public Command ItemSelectedCommand
        //{
        //    get { return this.itemSelectedCommand ?? (this.itemSelectedCommand = new Command(this.ItemSelected)); }
        //}

        /// <summary>
        /// Gets or sets the command that will be executed when the sort button is clicked.
        /// </summary>
        public Command SortCommand
        {
            get { return this.sortCommand ?? (this.sortCommand = new Command<SfButton>((btn) => this.SortClicked(btn))); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the filter button is clicked.
        /// </summary>
        public Command FilterCommand
        {
            get { return this.filterCommand ?? (this.filterCommand = new Command<object>((btn) => this.FilterClicked(btn))); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the Favourite button is clicked.
        /// </summary>
        public Command AddFavouriteCommand
        {
            get
            {
                return this.addFavouriteCommand ?? (this.addFavouriteCommand = new Command(this.AddFavouriteClicked));
            }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the AddToCart button is clicked.
        /// </summary>
        public Command AddToCartCommand
        {
            get { return this.addToCartCommand ?? (this.addToCartCommand = new Command(this.AddToCartClicked)); }
        }

        /// <summary>
        /// Gets or sets the command will be executed when the cart icon button has been clicked.
        /// </summary>
        public Command CartItemCommand
        {
            get { return this.cartItemCommand ?? (this.cartItemCommand = new Command(this.CartClicked)); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        //private void ItemSelected(object attachedObject)
        //{
        //    Product prod = attachedObject as Product;

        //    if (prod != null)
        //        Navigation.PushAsync(new Views.DetaljiBicikl.DetailPage(prod));
        //}

        /// <summary>
        /// Invoked when the items are sorted.
        /// </summary>
        /// <param name="btn">The button being clicked</param>
        private void SortClicked(SfButton btn)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the filter button is clicked.
        /// </summary>
        /// <param name="obj">The button being clicked</param>
        private void FilterClicked(object obj)
        {
            if (filteriUcitani && obj is SfButton btn)
            {
                SfPopupView popupView = new SfPopupView();
                popupView.ShowFilterPopUp("Zatvori", btn, this.FilterOptions, request, UcitajOpremu);
            }
        }

        /// <summary>
        /// Invoked when the favourite button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void AddFavouriteClicked(object obj)
        {
            if (obj is Product product)
                product.IsFavourite = !product.IsFavourite;
        }

        /// <summary>
        /// Invoked when the cart button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void AddToCartClicked(object obj)
        {

        }

        /// <summary>
        /// Invoked when cart icon button is clicked.
        /// </summary>
        private void CartClicked()
        {
            Navigation.PushAsync(new CartPage());
        }

        public async Task UpdateCartItemCount()
        {
            var count = await _serviceKorpaStavka.Get<int>(null, "GetBrojStavki");
            if (count > 0 || CartItemCount != null)
            {
                CartItemCount = count.ToString();
            }
        }

        #endregion
    }
}