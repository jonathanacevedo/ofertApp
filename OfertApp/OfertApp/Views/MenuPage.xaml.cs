using appOfertas.Models;
using Newtonsoft.Json;
using OfertApp.Models;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        String rol;
        public MenuPage()
        {
            InitializeComponent();
            CargarPersona();
            
            

            if (rol.Equals("Administrador"))
            {
                
                menuItems = new List<HomeMenuItem>
                {
                new HomeMenuItem { Id = MenuItemType.Inicio, Title = "Inicio", IconSource = "home_color.png" },
                new HomeMenuItem { Id = MenuItemType.Negocios, Title = "Mis Negocios", IconSource = "store_color.png" },
                new HomeMenuItem { Id = MenuItemType.Ofertas, Title = "Mis Ofertas", IconSource = "sale_color.png" },
                new HomeMenuItem { Id = MenuItemType.Config, Title = "Configuración", IconSource = "contacts_color.png" }
              };

            }

            if (rol.Equals("Cliente"))
            {
                menuItems = new List<HomeMenuItem>
                {
                new HomeMenuItem { Id = MenuItemType.Inicio, Title = "Inicio", IconSource = "home_color.png" },
            
                new HomeMenuItem { Id = MenuItemType.Config, Title = "Configuración", IconSource = "contacts_color.png" }
              };

            }
          

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }

        public async void CargarPersona()
        {
           
            var lista = await SecureStorage.GetAsync("auth");
           
            Personas personas = JsonConvert.DeserializeObject<Personas>(lista);
        
            labelNombre.Text = "Bienvenido/a " + personas.persona[0].nombre;
            rol = personas.persona[0].rol;
           
        }

        private async void Cerrar_Sesion(object sender, EventArgs e)
        {
            SecureStorage.Remove("auth");
            App.Current.MainPage = new Login();
            await Navigation.PushAsync(new Login());
        }
    }
}