using Newtonsoft.Json;
using OfertApp.Models;
using OfertApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OfertApp.ViewModels
{
    class ofertasViewModel: BaseViewModel
    {

        private HttpClient cliente = new HttpClient();

        public ObservableCollection<Oferta> Ofertas { set; get; }
        public Command LoadOfertasCommand { set; get; }

        public ofertasViewModel()
        {
            Ofertas = new ObservableCollection<Oferta>();
            LoadOfertasCommand = new Command(async () => await GetOfertas());
        }

        private async Task GetOfertas()
        { 
            Ofertas.Clear();
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
                        Ofertas.Add(oferta);
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
    }
}
