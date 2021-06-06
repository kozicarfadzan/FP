using FahrradladenPrinzenstrasse.Mobile.Views.Login;
using FahrradladenPrinzenstrasse.Mobile.Views.Signup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstrasse.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public MasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new MasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterDetailPageMasterMenuItem> MenuItems { get; set; }

            public MasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MasterDetailPageMasterMenuItem>(new[]
                {
                    new MasterDetailPageMasterMenuItem { Id = 0, Title = "Početna", TargetType = typeof(MainPage) },
                    new MasterDetailPageMasterMenuItem { Id = 1, Title = "Kupi Bicikl", TargetType = typeof(Views.KupiBicikl.CatalogListPage) },
                     new MasterDetailPageMasterMenuItem { Id = 2, Title = "Iznajmi Bicikl", TargetType = typeof(Views.IznajmiBicikl.CatalogListPage) },
                       new MasterDetailPageMasterMenuItem { Id = 3, Title = "Kupi Opremu", TargetType = typeof(Views.KupiOpremu.CatalogListPage) },
                       new MasterDetailPageMasterMenuItem { Id = 4, Title = "Kupi Dio", TargetType = typeof(Views.KupiDio.CatalogListPage) },
                    new MasterDetailPageMasterMenuItem { Id = 5, Title = "Servis", TargetType = typeof(Views.Servis.CatalogListPage) },
                    new MasterDetailPageMasterMenuItem { Id = 6, Title = "Košarica proizvoda", TargetType = typeof(Views.Korpa.CartPage) },
                    new MasterDetailPageMasterMenuItem { Id = 7, Title = "Košarica termina", TargetType = typeof(Views.Termini.CartPage) },
                    new MasterDetailPageMasterMenuItem { Id = 8, Title = "Rezervacije", TargetType = typeof(Views.Rezervacije.MyOrdersPage) },
                    new MasterDetailPageMasterMenuItem { Id = 9, Title = "Odjava", TargetType = typeof(Views.Login.SimpleLoginPage) },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}