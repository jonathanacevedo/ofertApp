using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FiltrosPage : Rg.Plugins.Popup.Pages.PopupPage
    {
		public FiltrosPage ()
		{
			InitializeComponent ();
		}

        private async void Filtrar_Clicked(object sender, EventArgs e)
        {
            if (restaurante.IsToggled)
            {
                MessagingCenter.Send<FiltrosPage, string>(this, "tipoNegocio", "Restaurante");

            }
            if (hotel.IsToggled)
            {
                MessagingCenter.Send<FiltrosPage, string>(this, "tipoNegocio", "Hotel");

            }
            if (bar.IsToggled)
            {
                MessagingCenter.Send<FiltrosPage, string>(this, "tipoNegocio", "Bar");

            }
            if (almacen.IsToggled)
            {
                MessagingCenter.Send<FiltrosPage, string>(this, "tipoNegocio", "Almacen");

            }
            if (otro.IsToggled)
            {
                MessagingCenter.Send<FiltrosPage, string>(this, "tipoNegocio", "Otro");
            }

            await PopupNavigation.Instance.PopAsync();
        }
    }
}