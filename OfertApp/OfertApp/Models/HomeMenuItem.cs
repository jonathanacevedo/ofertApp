using System;
using System.Collections.Generic;
using System.Text;

namespace OfertApp.Models
{
    public enum MenuItemType
    {
        Inicio,
        Negocios,
        Ofertas,
        Config
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        public string IconSource { get; internal set; }

    }
}
