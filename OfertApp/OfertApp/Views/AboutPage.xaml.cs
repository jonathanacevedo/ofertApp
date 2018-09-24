using appOfertas.Models;
using Newtonsoft.Json;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            mostrarUsuario();
        }

        private async void mostrarUsuario()
        {
            var oauthToken = await SecureStorage.GetAsync("auth");
            Personas personas = JsonConvert.DeserializeObject<Personas>(oauthToken);
            bienvenida.Text = personas.persona[0].nombre;
        }



        private void Button_Clicked_1(object sender, EventArgs e)
        {
            SecureStorage.Remove("auth");

        }
    }
}