using appOfertas.Models;
using Newtonsoft.Json;
using OfertApp.Models;
using OfertApp.Services;
using System;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        string nombre;
        public AboutPage()
        {
            InitializeComponent();
            MostrarUsuario();
        }

        private async void MostrarUsuario()
        {
            var oauthToken = await SecureStorage.GetAsync("auth");
            try
            {
                Personas personas = JsonConvert.DeserializeObject<Personas>(oauthToken);
                nombre = personas.persona[0].nombre;
            } catch
            {
                Console.WriteLine("Nombre de la persona de Google: "+oauthToken);
                nombre = oauthToken;
            }
            bienvenida.Text = nombre;
        }
        

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            SecureStorage.Remove("auth");
            App.Current.MainPage = new Login();
            await Navigation.PushAsync(new Login());
        }
    }
}