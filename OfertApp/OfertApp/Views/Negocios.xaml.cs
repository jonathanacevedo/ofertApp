using Newtonsoft.Json;
using OfertApp.Models;
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
	public partial class Negocios : ContentPage
	{

        negocios viewModel;

        private const string URL = "http://192.168.7.205:8091/negocios/eliminarPut";
        private HttpClient cliente = new HttpClient();

        public Negocios ()
		{
			InitializeComponent();

            //BindingContext = viewModel = new ItemsViewModel();
            BindingContext = viewModel = new negocios();  //NegociosViewModel
        }


        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var negocio = args.SelectedItem as Negocio;
            if (negocio == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(negocio)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Negocios.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        public async void Editar(object sender, SelectedItemChangedEventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = mi.CommandParameter as Negocio;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            //DisplayAlert("More Context Action", item.nombre + " more context action", "OK");
        }

        public async void Eliminar(object sender, EventArgs e)
        {

            var mi = ((MenuItem)sender);
            var negocioItem = mi.CommandParameter as Negocio;

            cliente.DefaultRequestHeaders.Add("Accept", "application/json");

            Negos negocios = new Negos();
            Negocio nego = new Negocio();

            List<Negocio> negocio = new List<Negocio>();

            nego.idnegocio = negocioItem.idnegocio;
            nego.parametro = "1152";
            nego.idadmin = "";
            nego.nombre = "";
            nego.nit = "";
            nego.email = "";
            nego.direccion = "";
            nego.telefono = "";
            nego.tipo = "";
            nego.ciudad = "";
            nego.detalle = "";
            nego.foto = "";
            nego.latitud = "";
            nego.longitud = "";

            negocio.Add(nego);

            negocios.negocio = negocio;

            var json = JsonConvert.SerializeObject(negocios);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await cliente.PutAsync(URL, content);
            var res = response.Content.ReadAsStringAsync();

            Console.WriteLine("Body: " + json);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("respuesta: " + res);
                await App.Current.MainPage.DisplayAlert("Correcto", "Negocio Eliminado", "OK");
            }
            else
            {
                Console.WriteLine("respuesta: " + res);
                await App.Current.MainPage.DisplayAlert("Error", "Algo salió mal", "OK");
            }
        }
    }
}

