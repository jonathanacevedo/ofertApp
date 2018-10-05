using OfertApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfertApp.ViewModels
{
    class NegocioDetalleViewModel : BaseViewModel
    {
        public Negocio Negocio { get; set; }
        public object Negocios { get; internal set; }

        public NegocioDetalleViewModel(Negocio negocio = null)
        {
            Title = negocio?.nombre;
            Negocio = negocio;
        }

      /* public static implicit operator NegocioDetalleViewModel(negocios v)
        {
            throw new NotImplementedException();
        }*/
    }
}
