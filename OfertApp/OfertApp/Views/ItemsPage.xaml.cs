using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using OfertApp.Models;
using OfertApp.Views;
using OfertApp.ViewModels;
using Xamarin.Forms.Maps;
using System.Net.Http;
using OfertApp.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {

        private HttpClient cliente = new HttpClient();
        public Label ofertas;
        public ScrollView scrollOfertas;
        public StackLayout stackOfertas;

        public ItemsPage()
        {
            var map = new Map(
            MapSpan.FromCenterAndRadius(
            new Position(6.2215477, -75.5722723), Distance.FromMiles(3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            MostrarNegocios(map);

            stackOfertas = new StackLayout
            {
                Spacing = 0,
                Orientation = StackOrientation.Horizontal
            };

            GetOfertas();


            scrollOfertas = new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal,
                Content = stackOfertas
            };

            var stackMapa = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Spacing = 0
            };

            var titulo = new Label
            {
                Text = "Bienvenido",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start
            };

            ofertas = new Label
            {
                Text = "Acá irían las ofertas",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = 18
            };

            var stack = new StackLayout { Spacing = 0 };

            stackMapa.Children.Add(map);
            stack.Children.Add(scrollOfertas);
            stack.Children.Add(stackMapa);

            Content = stack;
            //InitializeComponent();
        }


        public async void GetOfertas()
        {
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            var uri = new Uri(String.Format(Constants.IP + ":8092/ofertas/listar", String.Empty));
            var response = await cliente.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ofertas = JsonConvert.DeserializeObject<List<Oferta>>(content);
                    foreach (var oferta in ofertas)
                    {

                        var producto = new Label
                        {
                            Text = oferta.producto,
                            TextColor = Color.PaleVioletRed,
                            FontSize = 18
                        };
                        var tipo = new Label
                        {
                            Text = oferta.tipo,
                            FontSize = 16
                        };

                        var itemOferta = new StackLayout
                        {
                            Spacing = 0,
                            Orientation = StackOrientation.Vertical
                        };

                        itemOferta.Children.Add(producto);
                        itemOferta.Children.Add(tipo);

                        stackOfertas.Children.Add(itemOferta);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            } else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Servidor no disponible", "OK");
                Console.WriteLine("Error");
            }
        }

        private async void MostrarNegocios(Map map)
        {
            var posicion = new Position(Double.Parse("6.272363700000001"), Double.Parse("-75.59355340000002"));
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = posicion,
                Label = "Pin de Prueba",
                Address = "Detalle de Prueba"
            };

            map.Pins.Add(pin);

     
            /*cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            var uri = new Uri(String.Format(Constants.IP + ":8091/negocios/listar", String.Empty));
            var response = await cliente.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var negocios = JsonConvert.DeserializeObject<List<Negocio>>(content);


                    foreach (var negocio in negocios)
                    {
                        //Negocios.Add(negocio);
                        //OnPropertyChanged();

                        Console.WriteLine("Coordenadas del negocio: " + negocio.latitud);
                        
                        var posicionPrueba = new Position(Double.Parse(negocio.latitud), Double.Parse(negocio.longitud));
                        var pin = new Pin
                        {
                            Type = PinType.Place,
                            Position = posicionPrueba,
                            Label = negocio.nombre,
                            Address = negocio.detalle
                        };

                        map.Pins.Add(pin);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Servidor no disponible", "OK");
                Console.WriteLine("Error");
            }*/
        }


        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }
    }
}