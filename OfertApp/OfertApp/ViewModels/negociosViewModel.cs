using appOfertas.Models;
using Newtonsoft.Json;
using OfertApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OfertApp.ViewModels
{
    class negocios: BaseViewModel
    {
        private const string URL = "http://192.168.7.205:8050/orquestador/registrar/personas";
        private HttpClient cliente = new HttpClient();

        public ObservableCollection<Negocio> Negocios { set; get; }
        public Command LoadItemsCommand { get; set; }

        public negocios()
        {
            Negocios = new ObservableCollection<Negocio>();
            LoadItemsCommand = new Command(async () => await GetNegocios());
        }

        async Task GetNegocios()
        {
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            var uri = new Uri(String.Format("http://192.168.7.205:8091/negocios/listar", String.Empty));
            var response = await cliente.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                var content = await response.Content.ReadAsStringAsync();

                var negocios = JsonConvert.DeserializeObject<List<Negocio>>(content);
                
                foreach (var negocio in negocios)
                    {
                        Negocios.Add(negocio);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Servidor no disponible", "OK");
                Console.WriteLine("Error");
            }

        }
    }
}
