using System;

using FahrradladenPrinzenstraße.Mobile.Models;

namespace FahrradladenPrinzenstraße.Mobile.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
