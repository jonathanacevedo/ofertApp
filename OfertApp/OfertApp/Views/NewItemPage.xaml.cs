﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using OfertApp.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Xamarin.Essentials;
using appOfertas.Models;
using OfertApp.Services;
using Plugin.Media.Abstractions;
using Plugin.Media;
using System.Diagnostics;
using System.Threading.Tasks;
using Firebase.Storage;
using System.IO;

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        Negocios n = new Negocios();
        private const string URL = Constants.IP+":8091/negocios/registrar";
        private HttpClient cliente = new HttpClient();

        public Item Item { get; set; }

        //para la imagen
        public String urlImagen;
        MediaFile file;
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

        public NewItemPage(Negocios n)
        {
            InitializeComponent();
            this.n = n;
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
        public string ramdon()
        {
            Random obj = new Random();
            string posibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int longitud = posibles.Length;
            char letra;
            int longitudnuevacadena = 8;
            string nuevacadena = "";
            for (int i = 0; i < longitudnuevacadena; i++)
            {
                letra = posibles[obj.Next(longitud)];
                nuevacadena += letra.ToString();
            }
            return nuevacadena;
        }

        public async Task<string> StoreImages(Stream imageStream)
        {
            var stroageImage = await new FirebaseStorage("ofertas-1535298242523.appspot.com")
                .Child("XamarinImages").Child(ramdon()).PutAsync(imageStream);
            string imgurl = stroageImage;
            urlImagen = imgurl;
            imgChoosed.Text = urlImagen;
            //  Console.WriteLine("URL de la imagen: "+imgurl);
            return imgurl;
        }
    }
}