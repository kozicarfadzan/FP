using FahrradladenPrinzenstrasse.Mobile.ViewModels.PotvrdaNarudzbe;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstrasse.Mobile.Views.PotvrdaNarudzbe
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

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}