using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Mobile.Controls;
using FahrradladenPrinzenstrasse.Mobile.Models;
using FahrradladenPrinzenstrasse.Mobile.Views.Korpa;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;

namespace FahrradladenPrinzenstrasse.Mobile.ViewModels.KupiBicikl
{
    /// <summary>
    /// ViewModel for catalog page.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    [DataContract]
    public class CatalogPageViewModel : BaseViewModel
    {
        private readonly APIService _serviceBickl = new APIService("Bicikl");
        private readonly APIService _serviceKorpaStavka = new APIService("KorpaStavka");
        private readonly APIService _serviceProizvodjac = new APIService("Proizvodjac");
        private readonly APIService _serviceVelicinaOkvira = new APIService("VelicinaOkvira");
        private readonly APIService _serviceStarosnaGrupa = new APIService("StarosnaGrupa");
        private readonly APIService _serviceBrzina = new APIService("Brzina");
        private readonly APIService _serviceBoja = new APIService("Boja");
        private readonly INavigation Navigation;

        #region Fields

        private List<Category> filterOptions;

        private List<SortOption> sortOptions;


        private Model.Requests.BiciklSearchRequest request = new Model.Requests.BiciklSearchRequest();

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

            this.SortOptions = new List<SortOption>
            {
                new SortOption { Text = "Naziv - A-Z", Value = 1 },
                 new SortOption { Text = "Naziv - Z-A", Value = 2 },
                 new SortOption { Text = "Cijena - Najviša", Value = 3 },
                 new SortOption { Text = "Cijena - Najniža", Value = 4 },
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
        public List<SortOption> SortOptions
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

        public async Task UcitajBicikla()
        {
            var list = await _serviceBickl.Get<List<Model.Bicikl>>(request);
            Products.Clear();

            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    Products.Add(new Product
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

            this.filterOptions.Add(new Category
            {
                Name = "Stanje",
                FieldName = "Stanje",
                SubCategories = Enum.GetValues(typeof(Stanje))
                      .Cast<Stanje>()
                      .Where(x => x != Stanje.Korišteno)
                      .ToDictionary(t => (int)t, t => t.ToString()),
                FilterType = "checkbox"
            });
            this.filterOptions.Add(new Category
            {
                Name = "Nožna kočnica",
                FieldName = "NoznaKocnica",
                SubCategories = new Dictionary<int, string>
                    {
                        {-1, "Sve" },
                        {1, "Da" },
                        {0, "Ne" },
                    },
                FilterType = "radio"
            });

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

            var velicineOkvira = await _serviceVelicinaOkvira.Get<List<Model.VelicinaOkvira>>(null);
            this.filterOptions.Add(
                new Category
                {
                    Name = "Veličina okvira",
                    FieldName = "VelicinaOkviraId",
                    SubCategories = velicineOkvira
                       .ToDictionary(t => t.VelicinaOkviraId, t => t.Naziv),
                    FilterType = "checkbox"
                }
            );

            this.FilterOptions.Add(
                new Category
                {
                    Name = "Spolovi bicikla",
                    FieldName = "SpolBicikla",
                    SubCategories = Enum.GetValues(typeof(SpolBicikl))
                       .Cast<SpolBicikl>()
                       .ToDictionary(t => (int)t, t => t.ToString()),
                    FilterType = "checkbox"
                }
            );

            var starosneGrupe = await _serviceStarosnaGrupa.Get<List<Model.StarosnaGrupa>>(null);
            this.filterOptions.Add(
                new Category
                {
                    Name = "Starosne grupe",
                    FieldName = "StarosnaGrupaId",
                    SubCategories = starosneGrupe
                       .ToDictionary(t => t.StarosnaGrupaId, t => t.Naziv),
                    FilterType = "checkbox"
                }
            );

            var Suspenzije = new Dictionary<int, string>();
            Suspenzije.Add(-1, "Sve");
            foreach (var item in Enum.GetValues(typeof(Suspenzija)))
            {
                Suspenzije.Add((int)item, item.ToString());
            }

            this.FilterOptions.Add(
                new Category
                {
                    Name = "Suspenzija",
                    FieldName = "Suspenzija",
                    SubCategories = Suspenzije,
                    FilterType = "radio"
                }
            );

            var brzine = await _serviceBrzina.Get<List<int>>(new Model.Requests.BrzinaSearchRequest
            {
                SamoIznajmljivanje = false
            });
            this.filterOptions.Add(
                new Category
                {
                    Name = "Broj Brzina",
                    FieldName = "Brzina",
                    SubCategories = brzine
                       .ToDictionary(t => t, t => t.ToString()),
                    FilterType = "checkbox"
                }
            );

            var boje = await _serviceBoja.Get<List<Model.Boja>>(null);
            this.filterOptions.Add(
                new Category
                {
                    Name = "Boje",
                    FieldName = "BojaId",
                    SubCategories = boje
                       .ToDictionary(t => t.BojaId, t => t.Naziv),
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
        public Command ItemSelectedCommand
        {
            get { return this.itemSelectedCommand ?? (this.itemSelectedCommand = new Command(this.ItemSelected)); }
        }

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
        private void ItemSelected(object attachedObject)
        {
            Product prod = attachedObject as Product;

            if (prod != null)
                Navigation.PushAsync(new Views.DetaljiBicikl.DetailPage(prod));
        }

        /// <summary>
        /// Invoked when the items are sorted.
        /// </summary>
        /// <param name="obj">The button being clicked</param>
        private void SortClicked(object obj)
        {
            if (obj is SfButton btn)
            {
                SfPopupView popupView = new SfPopupView();
                popupView.ShowSortingPopUp("Zatvori", btn, this.SortOptions, request, UcitajBicikla);
            }
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
                popupView.ShowFilterPopUp("Zatvori", btn, this.FilterOptions, request, UcitajBicikla);
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