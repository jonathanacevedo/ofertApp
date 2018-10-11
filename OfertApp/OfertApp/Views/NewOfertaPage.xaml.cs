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
    public partial class NewOfertaPage : ContentPage
    {
        ofertaDetailViewModel viewModel;

        Ofertas n = new Ofertas();
        private const string URL = Constants.IP + ":8092/ofertas/registrar";
        private HttpClient cliente = new HttpClient();

        String idnegocio;

        public Item Item { get; set; }

        //para la imagen
        public String urlImagen;
        MediaFile file;
        Utilidades utilidades = new Utilidades();

        //fechas
        DateTime actual = DateTime.Now;
        String fecha_inicial;
        String fecha_final;
        DateTime fechaI;
        DateTime fechaF;

        public NewOfertaPage()
        {
            InitializeComponent();

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
            
            fecha_inicioFront.Date = actual;
            fecha_inicioFront.MinimumDate = actual;

            fecha_finFront.MinimumDate = actual;
            fecha_finFront.Date = actual;

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
            ofert.fecha_inicio = fecha_inicial;
            ofert.fecha_fin = fecha_final;
            ofert.foto = urlImagen;
            ofert.latitud = "";
            ofert.longitud = "";

            if (string.IsNullOrEmpty(ofert.producto))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter a product name", "Accept");

                return;
            }
            else if (string.IsNullOrEmpty(ofert.detalle))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter a detail", "Accept");

                return;
            }
            else if (string.IsNullOrEmpty(ofert.valor))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter a value", "Accept");

                return;
            }
            else if (string.IsNullOrEmpty(ofert.descuento))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must enter an ofer", "Accept");

                return;
            }
            else if (string.IsNullOrEmpty(ofert.foto))
            {
                await Application.Current.MainPage.DisplayAlert("error", "you must choose a photo", "Accept");

                return;
            }
            else if (fechaI > fechaF)
            {
                await Application.Current.MainPage.DisplayAlert("error", "No es posible que la fecha inicial sea mayor a la final", "cancel");
                return;    
            }



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

        // para las imagenes
        private async void btnPick_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
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

            var stroageImage = await new FirebaseStorage("ofertas-1535298242523.appspot.com")
                .Child("XamarinImages").Child(utilidades.ramdon()).PutAsync(imageStream);
            string imgurl = stroageImage;
            urlImagen = imgurl;
            imgChoosed.Text = urlImagen;
            //  Console.WriteLine("URL de la imagen: "+imgurl);
            return imgurl;
        }

        private void fecha_inicio_DateSelected(object sender, DateChangedEventArgs e)
        {
            fechaI = e.NewDate;
            fecha_inicial = fechaI.ToString("dd/MM/yyyy");
        }

        private void fecha_fin_DateSelected(object sender, DateChangedEventArgs e)
        {
            fechaF = e.NewDate;

            if (fechaI > fechaF)
            {
                App.Current.MainPage.DisplayAlert("error","No es posible que la fecha inicial sea mayor a la final", "cancel");
            }
            fecha_final = fechaF.ToString("dd/MM/yyyy");
          
        }
    }
}