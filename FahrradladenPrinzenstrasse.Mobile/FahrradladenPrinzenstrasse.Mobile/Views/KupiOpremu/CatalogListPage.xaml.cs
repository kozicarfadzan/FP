 
using FahrradladenPrinzenstrasse.Mobile.DataService;
using FahrradladenPrinzenstrasse.Mobile.ViewModels.KupiOpremu;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstrasse.Mobile.Views.KupiOpremu
{
    /// <summary>
    /// Page to show the catalog list. 
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogListPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogListPage" /> class.
        /// </summary>
        public CatalogListPage()
        {
            InitializeComponent();
            this.BindingContext = VM = new CatalogPageViewModel(Navigation);
        }

        public CatalogPageViewModel VM { get; set; }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await VM.UcitajOpremu().ConfigureAwait(false);
            await VM.UpdateCartItemCount().ConfigureAwait(false);
            await VM.UcitajFiltere().ConfigureAwait(false);
        }
    }
}