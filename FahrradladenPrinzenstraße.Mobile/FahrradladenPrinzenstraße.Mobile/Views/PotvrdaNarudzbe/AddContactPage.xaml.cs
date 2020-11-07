using FahrradladenPrinzenstraße.Mobile.ViewModels.PotvrdaNarudzbe;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstraße.Mobile.Views.PotvrdaNarudzbe
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddContactPage : ContentPage
    {
        private AddContactViewModel VM { get; }
        public AddContactPage()
        {
            InitializeComponent();

            BindingContext = VM = new AddContactViewModel(Navigation);
        }

    }
}