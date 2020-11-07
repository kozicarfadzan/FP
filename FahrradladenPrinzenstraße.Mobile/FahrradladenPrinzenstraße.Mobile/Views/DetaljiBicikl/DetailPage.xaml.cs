using FahrradladenPrinzenstraße.Mobile.ViewModels.DetaljiBicikl;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstraße.Mobile.Views.DetaljiBicikl
{
    /// <summary>
    /// The Detail page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage
    {
        private DetailPageViewModel VM;

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailPage" /> class.
        /// </summary>
        public DetailPage(Models.Product product)
        {
            InitializeComponent();
            this.BindingContext = VM = new DetailPageViewModel(product, Navigation);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await VM.UpdateCartItemCount();
        }

        /// <summary>
        /// Invoked when view size is changed.
        /// </summary>
        /// <param name="width">The Width</param>
        /// <param name="height">The Height</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //if (width > height)
            //{
            //    Rotator.ItemTemplate = (DataTemplate)this.Resources["LandscapeTemplate"];
            //}
            //else
            //{
            //    Rotator.ItemTemplate = (DataTemplate)this.Resources["PortraitTemplate"];
            //}
        }
    }
}