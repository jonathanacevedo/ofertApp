using appOfertas.Models;
using Newtonsoft.Json;
using OfertApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();

        String rol;
        public MainPage()
        {
            InitializeComponent();

            CargarPersona();
            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Inicio, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {

            if (rol.Equals("Administrador"))
            {
                if (!MenuPages.ContainsKey(id))
                {
                    switch (id)
                    {
                        case (int)MenuItemType.Inicio:
                            MenuPages.Add(id, new NavigationPage(new ItemsPage()));
                            break;
                        case (int)MenuItemType.Config:
                            MenuPages.Add(id, new NavigationPage(new AboutPage()));
                            break;
                        case (int)MenuItemType.Negocios:
                            MenuPages.Add(id, new NavigationPage(new Negocios()));
                            break;
                        case (int)MenuItemType.Ofertas:
                            MenuPages.Add(id, new NavigationPage(new Ofertas()));
                            break;
                    }
                }
            }
            if (rol.Equals("Cliente"))
            {
                if (!MenuPages.ContainsKey(id))
                {
                    switch (id)
                    {
                        case (int)MenuItemType.Inicio:
                            MenuPages.Add(id, new NavigationPage(new ItemsPage()));
                            break;
                        case (int)MenuItemType.Config:
                            MenuPages.Add(id, new NavigationPage(new AboutPage()));
                            break;
                       /* case (int)MenuItemType.Negocios:
                            MenuPages.Add(id, new NavigationPage(new Negocios()));
                            break;
                        case (int)MenuItemType.Ofertas:
                            MenuPages.Add(id, new NavigationPage(new Ofertas()));
                            break; */
                    }
                }
            }
            

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }

        public async void CargarPersona()
        {
            var lista = await SecureStorage.GetAsync("auth");
            Personas personas = JsonConvert.DeserializeObject<Personas>(lista);
             rol = personas.persona[0].rol;
        }

    }
}