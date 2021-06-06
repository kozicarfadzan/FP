using FahrradladenPrinzenstrasse.Mobile.DataService;
using FahrradladenPrinzenstrasse.Mobile.ViewModels.Rezervacije;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstrasse.Mobile.Views.Rezervacije
{
    /// <summary>
    /// Page to show the my order list. 
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyOrdersPage : ContentPage
    {
        private MyOrdersPageViewModel VM;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyOrdersPage" /> class.
        /// </summary>
        public MyOrdersPage()
        {
            InitializeComponent();
            this.BindingContext = VM = new MyOrdersPageViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.Init();
        }
    }
}