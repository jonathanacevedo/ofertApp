using Acr.UserDialogs;
using appOfertas.Models;
using Newtonsoft.Json;
using OfertApp.Models;
using OfertApp.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OfertApp.ViewModels
{
    class negocios: BaseViewModel, INotifyPropertyChanged
    {
        private const string URL = Constants.IP+":8050/orquestador/registrar/personas";
        private HttpClient cliente = new HttpClient();

        public ObservableCollection<Negocio> Negocios { set ; get; }
        public Command LoadItemsCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        String idPersona;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public negocios()
        {
            CargarPersona();
           

            Negocios = new ObservableCollection<Negocio>();
            LoadItemsCommand = new Command(async () => await GetNegocios());
        }

        async Task GetNegocios()
        {
                       
            if (IsBusy)
                return; 
                        
            IsBusy = true;
            
            OnPropertyChanged("cambio");
            Negocios.Clear();
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            var uri = new Uri(String.Format(Constants.IP+":8091/negocios/listar", String.Empty));
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
                        OnPropertyChanged();
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
        public async void CargarPersona()
        {

            var lista = await SecureStorage.GetAsync("auth");

            Personas personas = JsonConvert.DeserializeObject<Personas>(lista);
            idPersona = personas.persona[0].id;
          
        }
    }
}
