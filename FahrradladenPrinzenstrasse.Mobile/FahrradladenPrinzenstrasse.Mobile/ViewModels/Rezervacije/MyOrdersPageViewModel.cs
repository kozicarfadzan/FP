using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Mobile.Models.Rezervacije;
using FahrradladenPrinzenstrasse.Mobile.Views;
using FahrradladenPrinzenstrasse.Mobile.Views.DetaljiRezervacija;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FahrradladenPrinzenstrasse.Mobile.ViewModels.Rezervacije
{
    /// <summary>
    /// ViewModel for my orders page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class MyOrdersPageViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<Orders> orderDetails;

        private ObservableCollection<Orders> myOrders;

        private readonly APIService _serviceRezervacija = new APIService("Rezervacija");

        private Command itemSelectedCommand;
        private Command platiCommand;
        private Command otkaziCommand;
        private Command detaljiCommand;
        private INavigation Navigation;

        public MyOrdersPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
        }

        public async void Init()
        {
            await LoadOrders();
        }

        private async Task LoadOrders()
        {
            var request = new Model.Requests.RezervacijaSearchRequest
            {
                DatumOd = DatumOd,
                DatumDo = DatumDo
            };
            var list = await _serviceRezervacija.Get<List<Model.Rezervacija>>(request);
            var convertedList = new ObservableCollection<Orders>();
            foreach (var item in list)
            {
                var order = new Orders
                {
                    OrderID = item.RezervacijaId.ToString(),
                    Status = item.StanjeRezervacije.ToString().Replace("_", " "),
                    TotalPrice = item.UkupniIznos
                };

                if (item.IsServisRezervacija)
                    order.Name = "Servis";
                else if (item.IsTerminRezervacija)
                    order.Name = "Iznajmljivanje";
                else
                    order.Name = "Kupovina";

                order.Description = "Datum rezervacije: " + item.DatumRezervacije.ToShortDateString();
                if (item.DatumUplate != null)
                    order.Description += "\nDatum uplate: " + item.DatumUplate.Value.ToShortDateString();

                convertedList.Add(order);
            }

            MyOrders = convertedList;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the property that has been bound with list view, which displays the collection of orders from json.
        /// </summary>
        public ObservableCollection<Orders> MyOrders
        {
            get
            {
                return this.myOrders;
            }

            set
            {
                if (this.myOrders == value)
                {
                    return;
                }

                if(this.myOrders != null)
                    this.myOrders.Clear();
                this.myOrders = value;
                this.NotifyPropertyChanged();
                this.GetProducts(this.MyOrders);
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with a list view, which displays the order details in list.
        /// </summary>
        public ObservableCollection<Orders> OrderDetails
        {
            get
            {
                return this.orderDetails;
            }

            set
            {
                if (this.orderDetails == value)
                {
                    return;
                }

                this.orderDetails = value;
                this.NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Orders> OrderDetailsKupovina { get; set; } = new ObservableCollection<Orders>();
        public ObservableCollection<Orders> OrderDetailsServis { get; set; } = new ObservableCollection<Orders>();
        public ObservableCollection<Orders> OrderDetailsIznajmljivanje { get; set; } = new ObservableCollection<Orders>();

        private DateTime _datumOd = DateTime.Today.AddMonths(-1);

        public DateTime DatumOd
        {
            get { return _datumOd; }
            set
            {
                if (this._datumOd == value)
                {
                    return;
                }

                this._datumOd = value;
                this.NotifyPropertyChanged();
                LoadOrders();
            }
        }

        private DateTime _datumDo = DateTime.Today;

        public DateTime DatumDo
        {
            get { return _datumDo; }
            set
            {
                if (this._datumDo == value)
                {
                    return;
                }

                this._datumDo = value;
                this.NotifyPropertyChanged();
                LoadOrders();
            }
        }



        #endregion

        #region Command

        /// <summary>
        /// Gets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand
        {
            get
            {
                return this.itemSelectedCommand ?? (this.itemSelectedCommand = new Command(this.ItemSelected));
            }
        }

        /// <summary>
        /// Gets the command that will be executed when the pay button is clicked.
        /// </summary>
        public Command PlatiCommand
        {
            get
            {
                return this.platiCommand ?? (this.platiCommand = new Command<Orders>(async (order) => await this.PayNow(order)));
            }
        }

        /// <summary>
        /// Gets the command that will be executed when the cancel button is clicked.
        /// </summary>
        public Command OtkaziCommand
        {
            get
            {
                return this.otkaziCommand ?? (this.otkaziCommand = new Command<Orders>(async (order) => await this.Cancel(order)));
            }
        }

        /// <summary>
        /// Gets the command that will be executed when the pay button is clicked.
        /// </summary>
        public Command DetaljiCommand
        {
            get
            {
                return this.detaljiCommand ?? (this.detaljiCommand = new Command<Orders>(async (order) => await this.Detalji(order)));
            }
        }

        private async Task PayNow(Orders order)
        {
            var Rezervacija = await _serviceRezervacija.GetById<Model.Rezervacija>(order.OrderID);
            if (Rezervacija != null)
            {
                await Navigation.PushAsync(new StripeUplataPage(Rezervacija));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Greška", "Narudžba nije pronađena.", "OK");
            }
        }
        
        private async Task Cancel(Orders order)
        {
            var Rezervacija = await _serviceRezervacija.Update<bool>(int.Parse(order.OrderID), null, "OtkaziRezervaciju");
            if (Rezervacija != false)
            {
                order.Status = "Otkazana";
                await LoadOrders();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Greška", "Narudžba nije pronađena ili je već otkazana.", "OK");
            }
        }
        
        private async Task Detalji(Orders order)
        {
            await Navigation.PushAsync(new TransactionHistoryPage(int.Parse(order.OrderID)));
        }


        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        private void ItemSelected(object attachedObject)
        {
            // Do something
        }

        /// <summary>
        /// This method is used to get the ordered items from json.
        /// </summary>
        /// <param name="items">Ordered items</param>
        private void GetProducts(ObservableCollection<Orders> items)
        {
            this.OrderDetails = new ObservableCollection<Orders>();
            if (items != null && items.Count > 0)
            {
                this.OrderDetails = items;
                items.Where(x=>x.Name == "Kupovina").ForEach(x=>OrderDetailsKupovina.Add(x));
                items.Where(x=>x.Name == "Servis").ForEach(x=> OrderDetailsServis.Add(x));
                items.Where(x=>x.Name == "Iznajmljivanje").ForEach(x=> OrderDetailsIznajmljivanje.Add(x));
            }
        }

        #endregion
    }
}
