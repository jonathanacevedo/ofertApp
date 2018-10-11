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
	public partial class Ofertas : ContentPage
	{

        private const string URL = Constants.IP + ":8092/ofertas/eliminarPut";
        private HttpClient cliente = new HttpClient();

        ofertasViewModel viewModel;
		public Ofertas ()
		{
			InitializeComponent();

            BindingContext = viewModel = new ofertasViewModel();

        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var oferta = args.SelectedItem as Oferta;
            if (oferta == null)
                return;

            OfertaDetailPage pagEditar = new OfertaDetailPage(new ofertaDetailViewModel(oferta), this);
            var agregarNegocio = Navigation.PushModalAsync(new NavigationPage(pagEditar));

            //ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
                viewModel.LoadOfertasCommand.Execute(null);
        }

        private void Agregar_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Intentango agregar oferta");
        }

        public void Editar_Clicked(object sender, EventArgs e) {
            var mi = ((MenuItem)sender);
            var item = mi.CommandParameter as Oferta;

            OfertaEditPage pagEditar = new OfertaEditPage(new ofertaDetailViewModel(item), this);
            var agregarNegocio = Navigation.PushModalAsync(new NavigationPage(pagEditar));
        }

        public async void Eliminar_Clicked(object sender, EventArgs e)
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
                    actualizarVistaAsync();
                    await App.Current.MainPage.DisplayAlert("Correcto", "Oferta Eliminada", "OK");
                }
                else
                {
                    Console.WriteLine("respuesta: " + res);
                    await App.Current.MainPage.DisplayAlert("Error", "Algo salió mal", "OK");
                }

            }

        }

        public void actualizarVistaAsync()
        {
            Console.WriteLine("Actualizando vista...");
            viewModel.Ofertas.Clear();
            viewModel.LoadOfertasCommand.Execute(null);
        }
    }
}