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

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {

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


        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }
    }
}