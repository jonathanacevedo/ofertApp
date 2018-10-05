using Newtonsoft.Json;
using OfertApp.Models;
using OfertApp.Services;
using OfertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private const string URL = Constants.IP+":8091/negocios/eliminarPut";
        private HttpClient cliente = new HttpClient();

        public Negocios ()
		{
			InitializeComponent();

            BindingContext = viewModel = new negocios();  //NegociosViewModel
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var negocio = args.SelectedItem as Negocio;
            if (negocio == null)
                return;

            ItemDetailPage pagEditar = new ItemDetailPage(new ItemDetailViewModel(negocio), this);
            var agregarNegocio = Navigation.PushModalAsync(new NavigationPage(pagEditar));

            //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(negocio)));


            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        void AddItem_Clicked(object sender, EventArgs e)
        {

            var tcs = new TaskCompletionSource<bool>();
            NewItemPage pagina = new NewItemPage(this);
            var agregarNegocio = Navigation.PushModalAsync(new NavigationPage(pagina));
        }

        public void actualizarVistaAsync()
        {
            Console.WriteLine("Actualizando vista...");
            viewModel.Negocios.Clear();
            viewModel.LoadItemsCommand.Execute(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Negocios.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        public void Editar(object sender, SelectedItemChangedEventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = mi.CommandParameter as Negocio;

            NegocioEditPage pagEditar = new NegocioEditPage(new ItemDetailViewModel(item), this);
            var agregarNegocio = Navigation.PushModalAsync(new NavigationPage(pagEditar));

            //await Navigation.PushModalAsync(new NegocioEditPage(new ItemDetailViewModel(item)));
        }

        public async void Eliminar(object sender, EventArgs e)
        {

            var confirm = await DisplayAlert("Confirmación", "¿Está seguro de eliminar este negocio?", "Si", "No");
            Console.WriteLine("Respuesta: " + confirm);

            if (confirm)
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
                    viewModel.LoadItemsCommand.Execute(null);
                }
                else
                {
                    Console.WriteLine("respuesta: " + res);
                    await App.Current.MainPage.DisplayAlert("Error", "Algo salió mal", "OK");
                }
                
            }

         
        }

        private void ItemsListView_Refreshing(object sender, EventArgs e)
        {
            Console.WriteLine("Actualizando...");
        }
    }
}