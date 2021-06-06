using FahrradladenPrinzenstrasse.Mobile.ViewModels.MainPage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FahrradladenPrinzenstrasse.Mobile
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = VM = new MainPageViewModel(Navigation);
        }

        public MainPageViewModel VM { get; set; }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await VM.UcitajPopularneProizvode().ConfigureAwait(false);
        }
    }
}
