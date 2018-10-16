using Acr.UserDialogs;
using appOfertas.Models;
using Firebase.Storage;
using Newtonsoft.Json;
using OfertApp.Models;
using OfertApp.Services;
using OfertApp.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        //para la imagen
        public String urlImagen;
        MediaFile file;
        Utilidades utilidades = new Utilidades();
        String fotoVieja;
        public NegocioEditPage(ItemDetailViewModel viewModel, Negocios n)
        {
            InitializeComponent();
            this.n = n;

            BindingContext = this.viewModel = viewModel;
            fotoVieja = viewModel.Negocio.foto;
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
            nego.foto = urlImagen;

            Geolocalizacion geo = new Geolocalizacion();
            List<double> latLong = await geo.calcularCoordenadas(nego.direccion);
            if (latLong == null)
            {
                nego.latitud = "";
                nego.longitud = "";
                await DisplayAlert("Problema con la direccion", "No fue posible verficar la dirección", "OK");
                return;
            }
            else
            {
                nego.latitud = latLong.ElementAt(0).ToString().Replace(",", ".");
                nego.longitud = latLong.ElementAt(1).ToString().Replace(",", ".");
                //await DisplayAlert("Direccion correcta", "Latitud: " + nego.latitud + " Longitud: " + nego.longitud, "ok");
            }

            if (string.IsNullOrEmpty(nego.nombre))
            {
                await Application.Current.MainPage.DisplayAlert("error", "Campo nombre no puede estar vacio", "Accept");

                return;
            }

            else if (string.IsNullOrEmpty(nego.nit))
            {
                await Application.Current.MainPage.DisplayAlert("error", "Se debe ingresar un nit", "Accept");
                return;
            }

            else if (string.IsNullOrEmpty(nego.email))
            {
                await Application.Current.MainPage.DisplayAlert("error", "Debes ingresar un correo", "Accept");
                return;
            }

            else if (!utilidades.email_bien_escrito(nego.email))
            {
                await Application.Current.MainPage.DisplayAlert("error", "Correo no admitido", "Accept");
                return;
            }

            else if (string.IsNullOrEmpty(nego.telefono))
            {
                await Application.Current.MainPage.DisplayAlert("error", "Ingresa un telefono", "Accept");
                return;
            }

            else if (string.IsNullOrEmpty(nego.direccion))
            {
                await Application.Current.MainPage.DisplayAlert("error", "ingresa", "Accept");
                return;
            }

            else if (string.IsNullOrEmpty(nego.ciudad))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter a city", "Accept");
                return;
            }


            else if (string.IsNullOrEmpty(nego.detalle))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter a detail", "Accept");
                return;
            }


            else if (string.IsNullOrEmpty(nego.foto))
            {
                nego.foto = fotoVieja;
               
            }

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

        // para las imagenes
        private async void btnPick_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            imgChoosed.Text = "";
            try
            {
                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });
                if (file == null)
                    return;

                {
                    var imageStram = file.GetStream();

                };
                await StoreImages(file.GetStream());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        /*  private async void btnStore_Clicked(object sender, EventArgs e)
        {
            await StoreImages(file.GetStream());
        }*/
      

        public async Task<string> StoreImages(Stream imageStream)
        {
            using (UserDialogs.Instance.Loading("Subiendo Imagen"))
            {
                var stroageImage = await new FirebaseStorage("ofertas-1535298242523.appspot.com")
                .Child("XamarinImages").Child(utilidades.ramdon()).PutAsync(imageStream);
                string imgurl = stroageImage;
                urlImagen = imgurl;
                imgChoosed.Text = urlImagen;
                //  Console.WriteLine("URL de la imagen: "+imgurl);
                return imgurl;
            }
        }

    }
}