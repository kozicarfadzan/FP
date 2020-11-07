using FahrradladenPrinzenstraße.Mobile.DataService;
using FahrradladenPrinzenstraße.Mobile.ViewModels.Termini;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstraße.Mobile.Views.Termini
{
    /// <summary>
    /// Page to show the cart list. 
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage
    {
        private TerminiCartPageViewModel VM;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartPage" /> class.
        /// </summary>
        public CartPage()
        {
            InitializeComponent();
            this.BindingContext = VM = CartDataService.Instance.TerminiCartPageViewModel;
            VM.Navigation = Navigation;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await VM.InitCart();
        }
    }
}