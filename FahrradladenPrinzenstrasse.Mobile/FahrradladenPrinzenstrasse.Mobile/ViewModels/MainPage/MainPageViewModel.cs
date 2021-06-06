using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FahrradladenPrinzenstrasse.Mobile.ViewModels.MainPage
{
    public class MainPageViewModel: BaseViewModel
    {
        private INavigation navigation;
        private readonly APIService _serviceRecommender = new APIService("Recommender");

        public MainPageViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            this.ItemSelectedCommand = new Command(this.ItemSelected);
        }

        public async Task UcitajPopularneProizvode()
        {
            var proizvodi = await _serviceRecommender.Get<List<Model.PreporuceniProizvod>>(null, "PopularniProizvodi");
            PopularniProizvodi.Clear();
            foreach (var item in proizvodi)
            {
                PopularniProizvodi.Add(item);
            }
        }
        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        private void ItemSelected(object attachedObject)
        {
            // Do something
        }

        public Command ItemSelectedCommand { get; set; }
        public ObservableCollection<Model.PreporuceniProizvod> PopularniProizvodi { get; set; } = new ObservableCollection<Model.PreporuceniProizvod>();
    }
}
