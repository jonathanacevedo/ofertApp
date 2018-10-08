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
    public partial class OfertaEditPage : ContentPage
    {
        ofertaDetailViewModel viewModel;

        Ofertas n = new Ofertas();
        private const string URL = Constants.IP + ":8092/ofertas/editar";
        private HttpClient cliente = new HttpClient();


        internal OfertaEditPage(ofertaDetailViewModel viewModel, Ofertas n)
        {
            InitializeComponent();
            this.n = n;

            BindingContext = this.viewModel = viewModel;
        }

        public OfertaEditPage()
        {
            InitializeComponent();
            var oferta = new Oferta
            {
                producto = "negocio de Prueba",
                tipo = "Promocion"
            };

            viewModel = new ofertaDetailViewModel(oferta);
            BindingContext = viewModel;
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");

            Oferts ofertas = new Oferts();
            Oferta ofert = new Oferta();

            List<Oferta> oferta = new List<Oferta>();

            ofert.id = viewModel.Oferta.id;
            ofert.parametro = viewModel.Oferta.idnegocio;
            ofert.producto = producto.Text;
            ofert.tipo = (string)tipo.SelectedItem;
            ofert.detalle = detalle.Text;
            ofert.valor = valor.Text;
            ofert.descuento = descuento.Text;
            ofert.idnegocio = viewModel.Oferta.idnegocio;
            ofert.fecha_inicio = fecha_inicio.Text;
            ofert.fecha_fin = fecha_fin.Text;
            ofert.foto = "sinFoto";
            ofert.latitud = "";
            ofert.longitud = "";

            oferta.Add(ofert);

            ofertas.oferta = oferta;

            var json = JsonConvert.SerializeObject(ofertas);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await cliente.PutAsync(URL, content);
            var res = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(res);
                await App.Current.MainPage.DisplayAlert("Actualizar", "Editar Completado", "OK");
                n.actualizarVistaAsync();
                await Navigation.PopModalAsync();
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