using OfertApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfertApp.ViewModels
{
    class ofertaDetailViewModel: BaseViewModel
    {

        public Oferta Oferta { get; set; }
        public object Ofertas { get; internal set; } 

        public ofertaDetailViewModel(Oferta oferta = null)
        {
            Title = "";
            Oferta = oferta;
        }
    }
}