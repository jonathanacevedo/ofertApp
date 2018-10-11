using appOfertas.Models;
using Newtonsoft.Json;
using OfertApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistroPage : ContentPage
	{
        private const string URL = Constants.IP + ":8050/orquestador/registrar/personas";
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

            if (string.IsNullOrEmpty(person.nombre))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter a name", "Accept");

                return;
            }

            else if (string.IsNullOrEmpty(person.apellidos))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter a spellname", "Accept");
                return;
            }

            else if (string.IsNullOrEmpty(person.correo))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter an email", "Accept");
                return;
            }

            else if (! email_bien_escrito(person.correo))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter a correct email", "Accept");
                return;
            }

            else if (string.IsNullOrEmpty(person.contrasena))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter a password", "Accept");
                return;
            }
            else if (person.contrasena.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert("error", "password must have a minimum of 6 characters ", "Accept");
                return;
            }
            else if (string.IsNullOrEmpty(person.telefono))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter a phonenumber", "Accept");
                return;
            }


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

        private Boolean email_bien_escrito(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}