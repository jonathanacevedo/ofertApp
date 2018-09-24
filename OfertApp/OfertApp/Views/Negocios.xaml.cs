using OfertApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace OfertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Negocios : ContentPage
	{
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();

        public Negocios ()
		{
            var map = new Map(
           MapSpan.FromCenterAndRadius(
                   new Position(37, -122), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            var titulo = new Label
            {
                Text = "Bienvenido", HorizontalOptions = LayoutOptions.CenterAndExpand, 
                FontSize = 18, VerticalOptions = LayoutOptions.Start
            };

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(titulo);
            stack.Children.Add(map);
            Content = stack;
            //InitializeComponent ();
        }

    }
}