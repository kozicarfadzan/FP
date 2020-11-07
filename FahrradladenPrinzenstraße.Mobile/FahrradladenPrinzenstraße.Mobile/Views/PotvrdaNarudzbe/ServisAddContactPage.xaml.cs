using FahrradladenPrinzenstraße.Mobile.ViewModels.PotvrdaNarudzbe;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstraße.Mobile.Views.PotvrdaNarudzbe
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServisAddContactPage : ContentPage
    {
        private ServisAddContactViewModel VM { get; }
        public ServisAddContactPage(Model.Requests.ServisOdaberiTerminRequest request)
        {
            InitializeComponent();

            BindingContext = VM = new ServisAddContactViewModel(Navigation, request);
        }

    }
}