using FahrradladenPrinzenstraße.Mobile.ViewModels.PotvrdaNarudzbe;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstraße.Mobile.Views.PotvrdaNarudzbe
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IznajmiAddContactPage : ContentPage
    {
        private IznajmiAddContactViewModel VM { get; }
        public IznajmiAddContactPage()
        {
            InitializeComponent();

            BindingContext = VM = new IznajmiAddContactViewModel(Navigation);
        }

    }
}