using appOfertas.Models;
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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NegocioEditPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        Negocios n = new Negocios();
        private const string URL = Constants.IP + ":8091/negocios/editar";
        private HttpClient cliente = new HttpClient();

        public NegocioEditPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public NegocioEditPage()
        {
            InitializeComponent();
            var negocio = new Negocio
            {
                nombre = "negocio de Prueba",
                tipo = "Restaurante"
            };

            viewModel = new ItemDetailViewModel(negocio);
            BindingContext = viewModel;
        }

        private async void Editar_Clicked(object sender, EventArgs e)
        {
            var oauthToken = await SecureStorage.GetAsync("auth");
            Personas personas = JsonConvert.DeserializeObject<Personas>(oauthToken);

            cliente.DefaultRequestHeaders.Add("Accept", "application/json");

            Negos negocios = new Negos();
            Negocio nego = new Negocio();

            List<Negocio> negocio = new List<Negocio>();

            nego.idnegocio = viewModel.Negocio.idnegocio;
            nego.idadmin = personas.persona[0].id;
            nego.nombre = nom.Text;
            nego.nit = nit.Text;
            nego.email = email.Text;
            nego.direccion = direccion.Text;
            nego.telefono = telefono.Text;
            nego.tipo = (string)tipo.SelectedItem;
            nego.ciudad = ciudad.Text;
            nego.detalle = detalle.Text;
            nego.foto = "sinFoto";
            nego.latitud = "";
            nego.longitud = "";

            negocio.Add(nego);

            negocios.negocio = negocio;

            var json = JsonConvert.SerializeObject(negocios);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await cliente.PutAsync(URL, content);
            var res = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(res);
                await App.Current.MainPage.DisplayAlert("Actualizar", "Editar Completado", "OK");
                await Navigation.PopAsync();
                n.actualizarVistaAsync();
            }
            else
            {
                Console.WriteLine(res);
                await App.Current.MainPage.DisplayAlert("Error", "Algo salió mal", "OK");
                Console.WriteLine("Error");
            }
        }
    }
}