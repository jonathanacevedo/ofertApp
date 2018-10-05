using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using OfertApp.Models;
using OfertApp.ViewModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using OfertApp.Services;
using System.Text;

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        private const string URL = Constants.IP + ":8091/negocios/eliminarPut";
        private HttpClient cliente = new HttpClient();
        Negocios n = new Negocios();


        public ItemDetailPage(ItemDetailViewModel viewModel, Negocios n)
        {
            InitializeComponent();
            this.n = n;

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var negocio = new Negocio
            {
                nombre = "Item 1",
                tipo = "This is an item description."
            };

            viewModel = new ItemDetailViewModel(negocio);
            BindingContext = viewModel;
        }

       void Editar_Clicked(object sender, EventArgs e)
        {
            //var mi = ((MenuItem)sender);
            var item = sender as Negocio;

            NegocioEditPage pagEditar = new NegocioEditPage(new ItemDetailViewModel(viewModel.Negocio), n);
            var agregarNegocio = Navigation.PushModalAsync(new NavigationPage(pagEditar));

            //await Navigation.PushAsync(new NegocioEditPage(new ItemDetailViewModel(viewModel.Negocio)));
        }

        private async void Eliminar_Clicked(object sender, EventArgs e)
        {

            var confirm = await DisplayAlert("Confirmación", "¿Está seguro de eliminar este negocio?", "Si", "No");
            Console.WriteLine("Respuesta: " + confirm);

            if (confirm)
            {

                var negocioItem = viewModel.Negocio;

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
                    n.actualizarVistaAsync();
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