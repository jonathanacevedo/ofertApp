﻿using System;
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

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {

        private HttpClient cliente = new HttpClient();

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

            mostrarNegocios(map);

            var titulo = new Label
            {
                Text = "Bienvenido",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start
            };

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(titulo);

            stack.Children.Add(map);
            Content = stack;
            //InitializeComponent();
        }

        private async void mostrarNegocios(Map map)
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
                        //Negocios.Add(negocio);
                        //OnPropertyChanged();
                        
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
            }
        }


        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }
    }
}