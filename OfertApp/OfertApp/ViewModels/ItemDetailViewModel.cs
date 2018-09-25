using System;

using OfertApp.Models;

namespace OfertApp.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public object Items { get; internal set; }

        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }

        public static implicit operator ItemDetailViewModel(ItemsViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}
