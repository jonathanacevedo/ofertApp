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
using Acr.UserDialogs;

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {

        private HttpClient cliente = new HttpClient();
        public Label ofertas;
        public ScrollView scrollOfertas;
        public StackLayout stackOfertas;
        public StackLayout itemOferta;
        public StackLayout stack;
        private Map map;

        public ItemsPage()
        {            
            map = new Map(
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

            var toolbar = new ToolbarItem
            {
                Icon = "buscar1.png",
                Text = "Filtros"
            };
            stack = new StackLayout { Spacing = 0 };

            this.ToolbarItems.Add(toolbar);
            this.Title = "OfertApp";

            stackMapa.Children.Add(map);
            stack.Children.Add(scrollOfertas);
            stack.Children.Add(stackMapa);
            Content = stack;
                //InitializeComponent();
        }


        public void VaciarOfertas(int n)
        {
            for (int i = 0; i<n; i++)
            {
                stackOfertas.Children.RemoveAt(0);
            }
        }

        public async void GetOfertasPorNegocio(string idnegocio)
        {
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            var uri = new Uri(String.Format(Constants.IP + ":8092/ofertas/listar/negocio/"+idnegocio, String.Empty));
            var response = await cliente.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ofertas = JsonConvert.DeserializeObject<List<Oferta>>(content);
                    VaciarOfertas(stackOfertas.Children.Count);
                    Button btnTodos = new Button
                    {
                        Text = "<",
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = 10,
                        HeightRequest = 30,
                        WidthRequest = 30,
                        Padding = new Thickness(0, 0, 0, 0),
                        BackgroundColor = Color.FromHex("b42554".ToString()),
                        TextColor = Color.White
                    };

                    btnTodos.Clicked += (sender, args) =>
                    {
                        scrollOfertas.TranslationX = 0.0;
                        GetOfertas();
                    };

                    stackOfertas.Children.Add(btnTodos);
                    foreach (var oferta in ofertas)
                    {
                        itemOferta = new StackLayout
                        {
                            Spacing = 0,
                            Orientation = StackOrientation.Vertical,
                            HorizontalOptions = LayoutOptions.Center,
                            Padding = new Thickness(10, 10, 10, 10),
                            Children = {
                                new Label { Text = oferta.producto,
                                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                                        TextColor = Color.FromHex("b42554".ToString()), FontSize = 18,
                                        FontFamily = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android ? "Lobster.otf#Lobster" : null },
                                 new Image { Source = oferta.foto, HeightRequest=50, WidthRequest= 50},
                                new StackLayout //Layout Tipo
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    Children =
                                    {
                                    new Image { Source = "tipo.png", HeightRequest=10, WidthRequest=10},
                                    new Label { Text = "Tipo:",
                                        TextColor = Color.FromHex("b42554".ToString()),
                                        FontFamily = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android ? "Lobster.otf#Lobster" : null },

                                    new Label { Text = oferta.tipo},
                                    }
                                },
                                new StackLayout //Layout Descuento
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    Children =
                                    {
                                    new Image { Source = "oferta.png", HeightRequest=10, WidthRequest=10},
                                    new Label { Text = "Oferta:",
                                        TextColor = Color.FromHex("b42554".ToString()),
                                        FontFamily = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android ? "Lobster.otf#Lobster" : null },
                                    new Label { Text = oferta.descuento},
                                    }
                                }
                            }
                        };

                        var stackBotones = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.CenterAndExpand
                        };

                        Button btnUbicar = new Button
                        {
                            Text = "Ubicar",
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.Center,
                            FontSize = 10,
                            Padding = new Thickness(0, 0, 0, 0),
                            HeightRequest = 25,
                            BackgroundColor = Color.FromHex("b42554".ToString()),
                            TextColor = Color.White
                        };
                        Button btnNegocio = new Button
                        {
                            Text = "Negocio",
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalOptions = LayoutOptions.Center,
                            FontSize = 10,
                            HeightRequest = 25,
                            Padding = new Thickness(0, 0, 0, 0),
                            BackgroundColor = Color.FromHex("b42554".ToString()),
                            TextColor = Color.White
                        };

                        btnUbicar.Clicked += (sender, args) =>
                        {
                            GetCoordsNegocio(oferta.idnegocio);
                        };
                        btnNegocio.Clicked += (sender, args) =>
                        {
                            MostrarNegocioOferta(oferta.idnegocio);
                        };
                        stackBotones.Children.Add(btnNegocio);
                        stackBotones.Children.Add(btnUbicar);
                        itemOferta.Children.Add(stackBotones);
                        stackOfertas.Children.Add(itemOferta);
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
            }
        }


        public async void GetOfertas()
        {
            using (UserDialogs.Instance.Loading("Cargando..."))
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
                        VaciarOfertas(stackOfertas.Children.Count);
                        foreach (var oferta in ofertas)
                        {
                            itemOferta = new StackLayout
                            {
                                Spacing = 0,
                                Orientation = StackOrientation.Vertical,
                                HorizontalOptions = LayoutOptions.Center,
                                Padding = new Thickness(10, 10, 10, 10),
                                Children = {
                                new Label { Text = oferta.producto,
                                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                                        TextColor = Color.FromHex("b42554".ToString()), FontSize = 18,
                                        FontFamily = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android ? "Lobster.otf#Lobster" : null },
                                 new Image { Source = oferta.foto, HeightRequest=50, WidthRequest= 50},
                                new StackLayout //Layout Tipo
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    Children =
                                    {
                                    new Image { Source = "tipo.png", HeightRequest=10, WidthRequest=10},
                                    new Label { Text = "Tipo:",
                                        TextColor = Color.FromHex("b42554".ToString()),
                                        FontFamily = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android ? "Lobster.otf#Lobster" : null },

                                    new Label { Text = oferta.tipo},
                                    }
                                },
                                new StackLayout //Layout Descuento
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    Children =
                                    {
                                    new Image { Source = "oferta.png", HeightRequest=10, WidthRequest=10},
                                    new Label { Text = "Oferta:",
                                        TextColor = Color.FromHex("b42554".ToString()),
                                        FontFamily = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android ? "Lobster.otf#Lobster" : null },
                                    new Label { Text = oferta.descuento},
                                    }
                                }
                            }
                            };

                            var stackBotones = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                HorizontalOptions = LayoutOptions.CenterAndExpand
                            };

                            Button btnUbicar = new Button
                            {
                                Text = "Ubicar",
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.Center,
                                FontSize = 10,
                                Padding = new Thickness(0, 0, 0, 0),
                                HeightRequest = 25,
                                BackgroundColor = Color.FromHex("b42554".ToString()),
                                TextColor = Color.White
                            };
                            Button btnNegocio = new Button
                            {
                                Text = "Negocio",
                                VerticalOptions = LayoutOptions.Start,
                                HorizontalOptions = LayoutOptions.Center,
                                FontSize = 10,
                                HeightRequest = 25,
                                Padding = new Thickness(0, 0, 0, 0),
                                BackgroundColor = Color.FromHex("b42554".ToString()),
                                TextColor = Color.White
                            };

                            btnUbicar.Clicked += (sender, args) =>
                           {
                               GetCoordsNegocio(oferta.idnegocio);

                           };
                            btnNegocio.Clicked += (sender, args) =>
                            {
                                MostrarNegocioOferta(oferta.idnegocio);
                            };
                            stackBotones.Children.Add(btnNegocio);
                            stackBotones.Children.Add(btnUbicar);
                            itemOferta.Children.Add(stackBotones);
                            stackOfertas.Children.Add(itemOferta);
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
                }
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void GetCoordsNegocio(string idnegocio)
        {
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            var uri = new Uri(String.Format(Constants.IP + ":8091/negocios/listar/" + idnegocio, String.Empty));
            var response = await cliente.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var negocios = JsonConvert.DeserializeObject<Negos>(content);

                    map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(Convert.ToDouble(negocios.negocio[0].latitud.Replace(".", ",")), Convert.ToDouble(negocios.negocio[0].longitud.Replace(".", ","))), Distance.FromMiles(1)));
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
        }


        private async void MostrarNegocioOferta(string idnegocio)
        {
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            var uri = new Uri(String.Format(Constants.IP + ":8091/negocios/listar/"+idnegocio, String.Empty));
            var response = await cliente.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var negocios = JsonConvert.DeserializeObject<Negos>(content);
                    UserDetailPage pagDetalle = new UserDetailPage(new ItemDetailViewModel(negocios.negocio[0]));
                    var agregarNegocio = Navigation.PushModalAsync(new NavigationPage(pagDetalle));
                   
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
        }

        private async void MostrarNegocios(Map map)
        {
        cliente.DefaultRequestHeaders.Add("Accept", "application/json");
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
                        var latitud = negocio.latitud.Replace(".", ",");
                        var longitud = negocio.longitud.Replace(".", ",");
                        var posicionPrueba = new Position(Convert.ToDouble(latitud), Convert.ToDouble(longitud));
                        var pin = new Pin
                        {
                            Type = PinType.Place,
                            Position = posicionPrueba,
                            Label = negocio.nombre,
                            Address = negocio.tipo
                     };

                        pin.Clicked += (sender, args) =>
                        {
                            GetOfertasPorNegocio(negocio.idnegocio);
                            scrollOfertas.ScrollToAsync(0.0, 0.0, true);
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
         }
    }
        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }
    }
}
