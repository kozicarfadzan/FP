using FahrradladenPrinzenstrasse.Mobile.Models.DetaljiRezervacija;
using FahrradladenPrinzenstrasse.Mobile.ViewModels.DetaljiRezervacija;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstrasse.Mobile.Views.DetaljiRezervacija
{
    /// <summary>
    /// Page to show the transaction history.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionHistoryPage : ContentPage
    {
        private TransactionHistoryViewModel VM { get; }

        public TransactionHistoryPage(int RezervacijaId)
        {
            InitializeComponent();

            BindingContext = VM = new TransactionHistoryViewModel(RezervacijaId);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.LoadTransactions();
        }

        private async void ImageButton1_Clicked(object sender, System.EventArgs e)
        {
            var item = sender as ImageButton;
            var context = item.BindingContext as Transactions;
            await VM.RateStar(1, context);
        }

        private async void ImageButton2_Clicked(object sender, System.EventArgs e)
        {
            var item = sender as ImageButton;
            var context = item.BindingContext as Transactions;
            await VM.RateStar(2, context);
        }

        private async void ImageButton3_Clicked(object sender, System.EventArgs e)
        {
            var item = sender as ImageButton;
            var context = item.BindingContext as Transactions;
            await VM.RateStar(3, context);
        }

        private async void ImageButton4_Clicked(object sender, System.EventArgs e)
        {
            var item = sender as ImageButton;
            var context = item.BindingContext as Transactions;
            await VM.RateStar(4, context);
        }

        private async void ImageButton5_Clicked(object sender, System.EventArgs e)
        {
            var item = sender as ImageButton;
            var context = item.BindingContext as Transactions;
            await VM.RateStar(5, context);
        }
    }
}