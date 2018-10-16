using appOfertas.Models;
using Newtonsoft.Json;
using OfertApp.Models;
using OfertApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OfertApp.ViewModels
{
    class ofertasViewModel: BaseViewModel
    {

        private HttpClient cliente = new HttpClient();

        public ObservableCollection<Oferta> Ofertas { set; get; }
        public Command LoadOfertasCommand { set; get; }
        List<string> Negocios= new List<string>();
        string idPersona;

        public ofertasViewModel()
        {
            CargarPersona();
            Ofertas = new ObservableCollection<Oferta>();
           
            LoadOfertasCommand = new Command(async () => await GetOfertas());
        }

        private async void CargarPersona()
        {
            var lista = await SecureStorage.GetAsync("auth");

            Personas personas = JsonConvert.DeserializeObject<Personas>(lista);
            idPersona = personas.persona[0].id;
      
        }

        private async Task GetOfertas()
        {
          
            Ofertas.Clear();
            Negocios.Clear();
            
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            
            var uri2 = new Uri(String.Format(Constants.IP + ":8091/negocios/listar/admin/"+ idPersona, String.Empty));

            var response2 = await cliente.GetAsync(uri2);
          
            if (response2.IsSuccessStatusCode)
            {
                
                try
                {
                    var content = await response2.Content.ReadAsStringAsync();

                    var negocios = JsonConvert.DeserializeObject<List<Negocio>>(content);


                    foreach (var negocio in negocios)
                    {
                        Negocios.Add(negocio.idnegocio);
                        
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

            foreach (var idnegocio in Negocios)
            {

                var uri = new Uri(String.Format(Constants.IP + ":8092/ofertas/listar/negocio/" + idnegocio, String.Empty));
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
}
