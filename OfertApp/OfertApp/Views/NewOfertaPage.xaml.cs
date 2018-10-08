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
	public partial class NewOfertaPage : ContentPage
	{
        ofertaDetailViewModel viewModel;

        Ofertas n = new Ofertas();
        private const string URL = Constants.IP + ":8092/ofertas/registrar";
        private HttpClient cliente = new HttpClient();

        String idnegocio;

        public Item Item { get; set; }

        public NewOfertaPage ()
		{
			InitializeComponent ();

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        internal NewOfertaPage(String idnegocio)
        {
            InitializeComponent();

            this.idnegocio = idnegocio;
            //this.n = n;
            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();

        }

        private async void Agregar_Clicked(object sender, EventArgs e)
        {
            var oauthToken = await SecureStorage.GetAsync("auth");
            Personas personas = JsonConvert.DeserializeObject<Personas>(oauthToken);

            cliente.DefaultRequestHeaders.Add("Accept", "application/json");

            Oferts ofertas = new Oferts();
            Oferta ofert = new Oferta();

            List<Oferta> oferta = new List<Oferta>();

            ofert.producto = producto.Text;
            ofert.tipo = (string)tipo.SelectedItem;
            ofert.detalle = detalle.Text;
            ofert.valor = valor.Text;
            ofert.descuento = descuento.Text;
            ofert.idnegocio = this.idnegocio;
            ofert.fecha_inicio = fecha_inicio.Text;
            ofert.fecha_fin = fecha_fin.Text;
            ofert.foto = "sinFoto";
            ofert.latitud = "";
            ofert.longitud = "";

            oferta.Add(ofert);

            ofertas.oferta = oferta;
            
            var json = JsonConvert.SerializeObject(ofertas);
           
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Console.WriteLine("Body a enviar: " + content);
            var response = await cliente.PostAsync(URL, content);
            var res = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(res);
                await App.Current.MainPage.DisplayAlert("Registro", "Registro Completado", "OK");
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