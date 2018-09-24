using appOfertas.Models;
using Newtonsoft.Json;
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
	public partial class RegistroPage : ContentPage
	{
        private const string URL = "http://192.168.7.205:8050/orquestador/registrar/personas";
        private HttpClient cliente = new HttpClient();

		public RegistroPage ()
		{
			InitializeComponent ();
		}




        private async void Button_Clicked(object sender, EventArgs e)
        {

            cliente.DefaultRequestHeaders.Add("Accept", "application/json");

            Personas personas = new Personas();
            Persona person = new Persona();

            List<Persona> persona = new List<Persona>();

            person.nombre = nombre.Text;
            person.apellidos = apellidos.Text;
            person.correo = correo.Text;
            person.contrasena = contrasena.Text;
            person.telefono = telefono.Text;
            person.genero = (string)genero.SelectedItem;
            person.rol = (string)rol.SelectedItem;
            person.estado = "Activo";
            person.token = "Xamarin";

            persona.Add(person);

            personas.persona = persona;

            var json = JsonConvert.SerializeObject(personas);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await cliente.PostAsync(URL, content);

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                Console.WriteLine(res);
                await App.Current.MainPage.DisplayAlert("Registro", "Registro Completado", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email o contraseña incorrectos", "OK");
                Console.WriteLine("Error");
            }
            Console.WriteLine(personas.persona[0].nombre);
        }
    }
}