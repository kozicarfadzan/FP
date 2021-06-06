using FahrradladenPrinzenstrasse.Mobile.DataService;
using FahrradladenPrinzenstrasse.Mobile.ViewModels.Korpa;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstrasse.Mobile.Views.Korpa
{
    /// <summary>
    /// Page to show the cart list. 
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage
    {
        private CartPageViewModel VM;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartPage" /> class.
        /// </summary>
        public CartPage()
        {
            InitializeComponent();
            this.BindingContext = VM = CartDataService.Instance.CartPageViewModel;
            VM.Navigation = Navigation;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await VM.InitCart();
        }
    }
}