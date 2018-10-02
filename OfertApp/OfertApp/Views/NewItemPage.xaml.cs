using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using OfertApp.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Xamarin.Essentials;
using appOfertas.Models;

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        private const string URL = "http://192.168.10.53:8091/negocios/registrar";
        private HttpClient cliente = new HttpClient();

        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var oauthToken = await SecureStorage.GetAsync("auth");
            Personas personas = JsonConvert.DeserializeObject<Personas>(oauthToken);

            cliente.DefaultRequestHeaders.Add("Accept", "application/json");

            Negos negocios = new Negos();
            Negocio nego = new Negocio();

            List<Negocio> negocio = new List<Negocio>();

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