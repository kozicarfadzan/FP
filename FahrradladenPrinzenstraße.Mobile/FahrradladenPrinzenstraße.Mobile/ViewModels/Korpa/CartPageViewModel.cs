using System.Collections.ObjectModel;
using FahrradladenPrinzenstraße.Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Runtime.Serialization;
using FahrradladenPrinzenstraße.Mobile.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;
using FahrradladenPrinzenstraße.Mobile.Views.PotvrdaNarudzbe;
using FahrradladenPrinzenstraße.Mobile.Views.KupiOpremu;

namespace FahrradladenPrinzenstraße.Mobile.ViewModels.Korpa
{
    /// <summary>
    /// ViewModel for cart page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class CartPageViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<Product> cartDetails = new ObservableCollection<Product>();

        private double totalPrice;

        private ObservableCollection<Product> products;

        private Command placeOrderCommand;

        private Command removeCommand;

        private Command quantitySelectedCommand;

        private Command variantSelectedCommand;

        private Command backButtonCommand;

        private readonly APIService _serviceKorpa = new APIService("KorpaStavka");


        #endregion

        public CartPageViewModel()
        {

        }

        public async Task InitCart()
        {
            var list = await _serviceKorpa.Get<List<Model.KorpaStavka>>(null);
            var products = new ObservableCollection<Product>();

            foreach (var item in list)
            {
                string ProductName = null, ProductDescription = null;
                double ProductPrice = 0d;
                byte[] ProductPhoto = null;
                int TotalQuantity = 0;
                if (item.Bicikl != null)
                {
                    ProductName = item.Bicikl.PuniNaziv;
                    if (item.Bicikl.Stanje == Model.Stanje.Korišteno)
                        ProductPrice = item.Bicikl.CijenaPoDanu.Value;
                    else
                        ProductPrice = item.Bicikl.Cijena.Value;
                    ProductPhoto = item.Bicikl.Slika;
                    ProductDescription = item.Bicikl.Opis;
                    TotalQuantity = item.Bicikl.Kolicina;
                }
                else if (item.Dio != null)
                {
                    ProductName = item.Dio.Naziv;
                    ProductPrice = item.Dio.Cijena;
                    ProductPhoto = item.Dio.Slika;
                    ProductDescription = item.Dio.Opis;
                    TotalQuantity = item.Dio.Kolicina;
                }
                else if (item.Oprema != null)
                {
                    ProductName = item.Oprema.Naziv;
                    ProductPrice = item.Oprema.Cijena;
                    ProductPhoto = item.Oprema.Slika;
                    ProductDescription = item.Oprema.Opis;
                    TotalQuantity = item.Oprema.Kolicina;
                }

                var QuantitiesList = new List<object>();
                for (int i = 1; i <= TotalQuantity; i++)
                {
                    QuantitiesList.Add(i);
                }


                products.Add(new Product
                {
                    Id = item.KorpaStavkaId,
                    Name = ProductName,
                    ActualPrice = ProductPrice,
                    Bicikl = item.Bicikl,
                    Oprema = item.Oprema,
                    Dio = item.Dio,
                    Slika = ProductPhoto,
                    TotalQuantity = item.Kolicina,
                    Description = ProductDescription,
                    Quantities = QuantitiesList
                });
            }
            this.Products = products;
        }

        #region Public properties

        public INavigation Navigation { get; set; }

        /// <summary>
        /// Gets or sets the property that has been bound with a list view, which displays the cart details.
        /// </summary>
        public ObservableCollection<Product> CartDetails
        {
            get
            {
                return this.cartDetails;
            }

            set
            {
                if (this.cartDetails == value)
                {
                    return;
                }

                this.cartDetails = value;
                this.NotifyPropertyChanged();
                KosaricaImaStavke = this.cartDetails.Count > 0;
                KosaricaNemaStavke = !KosaricaImaStavke;
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with label, which displays the total price.
        /// </summary>
        public double TotalPrice
        {
            get
            {
                return this.totalPrice;
            }

            set
            {
                if (this.totalPrice == value)
                {
                    return;
                }

                this.totalPrice = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with list view, which displays the collection of products from json.
        /// </summary>

        public ObservableCollection<Product> Products
        {
            get
            {
                return this.products;
            }

            set
            {
                if (this.products == value)
                {
                    return;
                }
                this.products = value;
                this.NotifyPropertyChanged();
                KosaricaImaStavke = this.products.Count > 0;
                KosaricaNemaStavke = !KosaricaImaStavke;
                this.GetProducts(Products);
                this.UpdatePrice();
            }
        }

        private bool _kosaricaImaStavke = false;

        public bool KosaricaImaStavke
        {
            get { return _kosaricaImaStavke; }
            set
            {
                if (this._kosaricaImaStavke == value)
                {
                    return;
                }
                this._kosaricaImaStavke = value;
                this.NotifyPropertyChanged();
            }
        }

        
        private bool _kosaricaNemaStavke = true;

        public bool KosaricaNemaStavke
        {
            get { return _kosaricaNemaStavke; }
            set
            {
                if (this._kosaricaNemaStavke == value)
                {
                    return;
                }
                this._kosaricaNemaStavke = value;
                this.NotifyPropertyChanged();
            }
        }




        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that will be executed when the Edit button is clicked.
        /// </summary>
        public Command PlaceOrderCommand
        {
            get { return this.placeOrderCommand ?? (this.placeOrderCommand = new Command(this.PlaceOrderClicked)); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the Remove button is clicked.
        /// </summary>
        public Command RemoveCommand
        {
            get { return this.removeCommand ?? (this.removeCommand = new Command(this.RemoveClicked)); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the quantity is selected.
        /// </summary>
        public Command QuantitySelectedCommand
        {
            get { return this.quantitySelectedCommand ?? (this.quantitySelectedCommand = new Command(this.QuantitySelected)); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the variant is selected.
        /// </summary>
        public Command VariantSelectedCommand
        {
            get { return this.variantSelectedCommand ?? (this.variantSelectedCommand = new Command(this.VariantSelected)); }
        }

        /// <summary>
        /// Gets or sets the command is executed when the back button is clicked.
        /// </summary>
        public Command BackButtonCommand
        {
            get { return this.backButtonCommand ?? (this.backButtonCommand = new Command(async () => await BackButtonClicked())); }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void PlaceOrderClicked(object obj)
        {
            Navigation.PushAsync(new AddContactPage());
        }

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void RemoveClicked(object obj)
        {
            if (obj is Product product)
            {
                var removed = await _serviceKorpa.Delete<bool>(product.Id);
                if(removed == false)
                {
                    SfPopupView sfPopupView = new SfPopupView();
                    sfPopupView.ShowPopUp(content: "Item could not be removed from cart.", buttonText: "OK");
                    return;
                }
                this.CartDetails.Remove(product);
                this.UpdatePrice();

                if (this.CartDetails.Count == 0)
                {
                    SfPopupView sfPopupView = new SfPopupView();
                    sfPopupView.ShowPopUp(content: "Košarica je prazna!", buttonText: "Nastavite kupovinu", navigation: Navigation, pageToNavigate: typeof(CatalogListPage));
                }
            }
        }

        /// <summary>
        /// Invoked when the quantity is selected.
        /// </summary>
        /// <param name="selectedItem">The Object</param>
        private async void QuantitySelected(object selectedItem)
        {
            var item = selectedItem as Product;
            var updatedStavka = await _serviceKorpa.Update<Model.KorpaStavka>(item.Id, new Model.Requests.KorpaStavkaInsertRequest
            {
                BiciklId = item.Bicikl?.BiciklId,
                DioId = item.Dio?.DioId,
                OpremaId = item.Oprema?.OpremaId,
                Kolicina = item.TotalQuantity
            });
            if(updatedStavka == null)
            {
                RemoveClicked(item);
                return;
            }

            this.UpdatePrice();
        }

        /// <summary>
        /// Invoked when the variant is selected.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void VariantSelected(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when an back button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async Task BackButtonClicked()
        {
            await Navigation.PopAsync();
        }

        /// <summary>
        /// This method is used to get the products from json.
        /// </summary>
        /// <param name="Products">The Products</param>
        private void GetProducts(ObservableCollection<Product> Products)
        {
            this.CartDetails = new ObservableCollection<Product>();
            if (Products != null && Products.Count > 0)
                this.CartDetails = Products;
        }

        /// <summary>
        /// This method is used to update the price amount.
        /// </summary>
        private void UpdatePrice()
        {
            this.ResetPriceValue();

            if (this.CartDetails != null && this.CartDetails.Count > 0)
            {
                foreach (var cartDetail in this.CartDetails)
                {
                    if (cartDetail.TotalQuantity == 0)
                        cartDetail.TotalQuantity = 1;
                    this.TotalPrice += (cartDetail.ActualPrice * cartDetail.TotalQuantity);
                }

            }
        }

        /// <summary>
        /// This method is used to reset the price amount.
        /// </summary>
        private void ResetPriceValue()
        {
            this.TotalPrice = 0;
        }

        #endregion
    }
}
