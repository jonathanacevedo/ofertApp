using appOfertas.Models;
using Newtonsoft.Json;
using OfertApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OfertApp.ViewModels
{
    class negocios: BaseViewModel, INotifyPropertyChanged
    {
        private const string URL = "http://192.168.7.205:8050/orquestador/registrar/personas";
        private HttpClient cliente = new HttpClient();

        public ObservableCollection<Negocio> Negocios { set; get; }
        public Command LoadItemsCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string Negocios)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Negocios));
            Console.WriteLine("Hubo un cambio");
        }

        public negocios()
        {
            Negocios = new ObservableCollection<Negocio>();
            LoadItemsCommand = new Command(async () => await GetNegocios());
        }

        async Task GetNegocios()
        {

            if (IsBusy)
                return;

            IsBusy = true;
            
            Console.WriteLine("Intentando coger datos");
            OnPropertyChanged("cambio");
            Negocios.Clear();
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
                } finally {
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
