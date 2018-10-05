using System;

using OfertApp.Models;

namespace OfertApp.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Negocio Negocio { get; set; }
        public object Negocios { get; internal set; }

        public ItemDetailViewModel(Negocio negocio = null)
        {
            Title = negocio?.nombre;
            Negocio = negocio;
        }

        internal void PropertyChanged()
        {
            Console.WriteLine("Hubo un cambio");
        }

        /*public static implicit operator ItemDetailViewModel(negocios v)
        {
            throw new NotImplementedException();
        }*/
    }
}
