using System;
using System.Collections.Generic;
using System.Text;

namespace OfertApp.Models
{
    class Oferta
    {
        public string id { set; get; }
        public string producto { get; set; }
        public string detalle { set; get; }
        public string valor { get; set; }
        public string descuento { set; get; }
        public string foto { get; set; }
        public string idnegocio { set; get; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { set; get; }
        public string latitud { get; set; }
        public string longitud { set; get; }
        public string tipo { get; set; }
        public string parametro { get; set; }

        public Oferta() { }
    }
}
