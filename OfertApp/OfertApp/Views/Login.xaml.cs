using appOfertas.Models;
using Newtonsoft.Json;
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
	public partial class Login : ContentPage
	{
		public Login ()
		{
			InitializeComponent ();
		}

        private async Task Button_Clicked2(object sender, EventArgs e)
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            String user = email.Text;
            String pass = password.Text;
            var uri = new Uri(String.Format("http://192.168.7.205:8090/personas/loguear?usuario=" + user + "&password=" + pass, String.Empty));
            var response = await cliente.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Personas personas = JsonConvert.DeserializeObject<Personas>(content);
                Console.WriteLine(content);
                Console.WriteLine("Bienvenido " + personas.persona[0].nombre);
                try
                {
                    await SecureStorage.SetAsync("auth", content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error " + ex);
                }
                App.Current.MainPage = new MainPage();
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email o contraseña incorrectos", "OK");
                Console.WriteLine("Error");
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistroPage());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            String user = email.Text;
            String pass = password.Text;
            var uri = new Uri(String.Format("http://192.168.7.205:8090/personas/loguear?usuario=" + user + "&password=" + pass, String.Empty));
            var response = await cliente.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Personas personas = JsonConvert.DeserializeObject<Personas>(content);
                Console.WriteLine(content);
                Console.WriteLine("Bienvenido " + personas.persona[0].nombre);
                try
                {
                    await SecureStorage.SetAsync("auth", content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error " + ex);
                }
                App.Current.MainPage = new MainPage();
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email o contraseña incorrectos", "OK");
                Console.WriteLine("Error");
            }
        }
    }
}