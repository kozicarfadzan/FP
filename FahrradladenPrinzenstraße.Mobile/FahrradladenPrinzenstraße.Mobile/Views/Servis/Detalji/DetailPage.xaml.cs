using FahrradladenPrinzenstraße.Mobile.ViewModels.Servis;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FahrradladenPrinzenstraße.Mobile.Views.Servis.Detalji
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
            this.BindingContext = VM = new DetailPageViewModel(product, Navigation, blackoutDates, calendar);
        }

        List<DateTime> blackoutDates = new List<DateTime>();

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await VM.LoadBlackoutDates();
        }

        /// <summary>
        /// Invoked when view size is changed.
        /// </summary>
        /// <param name="width">The Width</param>
        /// <param name="height">The Height</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }

        private void calendar_OnMonthCellLoaded(object sender, Syncfusion.SfCalendar.XForms.MonthCellLoadedEventArgs e)
        {
            var dayOfWeek = e.Date.DayOfWeek;
            if (dayOfWeek == DayOfWeek.Sunday)
            {
                if(blackoutDates.Contains(e.Date) == false)
                {
                    blackoutDates.Add(e.Date);
                    calendar.BlackoutDates = blackoutDates;
                }
            }
        }

        private void SfNumericUpDown_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            VM.CalendarSelectionChanged();
        }
    }
}