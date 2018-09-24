using System;
using System.Collections.Generic;
using System.Text;

namespace appOfertas.Models
{
    public class Persona
    {
        public string id { set; get; }
        public string nombre { set; get; }
        public string apellidos { set; get; }
        public string correo { set; get; }
        public string contrasena { set; get; }
        public string telefono { set; get; }
        public string genero { set; get; }
        public string rol { set; get; }
        public string estado { set; get; }
        public string token { set; get; }
        public Persona() { }

        public static implicit operator List<object>(Persona v)
        {
            throw new NotImplementedException();
        }
    }
}
