using Newtonsoft.Json;
using OfertApp.Models;
using OfertApp.Services;
using OfertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OfertaDetailPage : ContentPage
	{
        ofertaDetailViewModel viewModel;

        private const string URL = Constants.IP + ":8092/ofertas/eliminarPut";
        private HttpClient cliente = new HttpClient();
        Ofertas n = new Ofertas();

        internal OfertaDetailPage (ofertaDetailViewModel viewModel, Ofertas n)
		{
			InitializeComponent ();
            this.n = n;

            BindingContext = this.viewModel = viewModel;
        }

        public OfertaDetailPage()
        {
            InitializeComponent();

            var oferta = new Oferta
            {
                producto = "Item 1",
                tipo = "This is an item description."
            };

            viewModel = new ofertaDetailViewModel(oferta);
            BindingContext = viewModel;
        }

        private async void Eliminar_Clicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Confirmación", "¿Está seguro de eliminar este negocio?", "Si", "No");
            if (confirm)
            {
                var mi = ((MenuItem)sender);
                var ofertaItem = mi.CommandParameter as Oferta;

                cliente.DefaultRequestHeaders.Add("Accept", "application/json");

                Oferts ofertas = new Oferts();
                Oferta ofer = new Oferta();

                List<Oferta> oferta = new List<Oferta>();

                ofer.id = ofertaItem.id;
                ofer.parametro = "1";
                ofer.producto = "";
                ofer.detalle = "";
                ofer.valor = "";
                ofer.descuento = "";
                ofer.foto = "";
                ofer.idnegocio = "";
                ofer.fecha_inicio = "";
                ofer.fecha_fin = "";
                ofer.latitud = "";
                ofer.longitud = "";
                ofer.tipo = "";

                oferta.Add(ofer);


                ofertas.oferta = oferta;

                var json = JsonConvert.SerializeObject(ofertas);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await cliente.PutAsync(URL, content);
                var res = response.Content.ReadAsStringAsync();

                Console.WriteLine("Body: " + json);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("respuesta: " + res);
                    n.actualizarVistaAsync();
                    await App.Current.MainPage.DisplayAlert("Correcto", "Oferta Eliminada", "OK");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    Console.WriteLine("respuesta: " + res);
                    await App.Current.MainPage.DisplayAlert("Error", "Algo salió mal", "OK");
                }

            }
        }
    }
}