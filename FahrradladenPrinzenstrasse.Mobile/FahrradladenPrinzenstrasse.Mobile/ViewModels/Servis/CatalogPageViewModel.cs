using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Mobile.Controls;
using FahrradladenPrinzenstrasse.Mobile.Models;
using FahrradladenPrinzenstrasse.Mobile.Views.Termini;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;

namespace FahrradladenPrinzenstrasse.Mobile.ViewModels.Servis
{
    /// <summary>
    /// ViewModel for catalog page.
    /// </summary>
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    [DataContract]
    public class CatalogPageViewModel : BaseViewModel
    {
        private readonly APIService _serviceServis = new APIService("Servis");

        private readonly INavigation Navigation;

        #region Fields

        private Command addFavouriteCommand;

        private Command itemSelectedCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="CatalogPageViewModel" /> class.
        /// </summary>
        public CatalogPageViewModel(INavigation navigation)
        {
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

        public async Task UcitajServise()
        {
            var list = await _serviceServis.Get<List<Model.Servis>>(null);
            Products.Clear();

            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    Products.Add(new Product
                    {
                        Id = item.ServisId,
                        ActualPrice = item.Cijena,
                        Name = item.Naziv,
                        Description = item.Opis,
                        Servis = item
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

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand
        {
            get { return this.itemSelectedCommand ?? (this.itemSelectedCommand = new Command<object>(this.ItemSelected)); }
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

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        private async void ItemSelected(object attachedObject)
        {
            Product prod = attachedObject as Product;

            if (prod != null)
                await Navigation.PushAsync(new Views.Servis.Detalji.DetailPage(prod));
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

        #endregion
    }
}