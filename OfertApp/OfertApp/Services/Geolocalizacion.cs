using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace OfertApp.Services
{
    class Geolocalizacion
    {
        private Placemark marcador { get; set; }

        public Geolocalizacion()
        {
            this.marcador = new Placemark();
        }

        public async Task<List<double>> calcularCoordenadas(String direccion)
        {
            List<double> latLong = null;
            try
            {
                var localizaciones = await Geocoding.GetLocationsAsync(direccion + " AN CO");
                var localizacion = localizaciones?.FirstOrDefault();
                if (localizacion != null)
                {
                    double latitud = localizacion.Latitude;
                    double longitud = localizacion.Longitude;

                    latLong = new List<double>();
                    latLong.Insert(0, latitud);
                    latLong.Insert(1, longitud);
                }
            }
            catch (Exception e)
            {

            }
            return latLong;
        }

        public async void obtenerMarcador(double latitud, double longitud)
        {
            try
            {
                var marcadores = await Geocoding.GetPlacemarksAsync(latitud, longitud);
                this.marcador = marcadores?.FirstOrDefault();
            }
            catch (Exception e)
            {

            }
        }

        public void imprimirMarcador(Placemark marcador)
        {
            var direccion =
            $"AdminArea:       {marcador.AdminArea}\n" +
            $"CountryCode:     {marcador.CountryCode}\n" +
            $"CountryName:     {marcador.CountryName}\n" +
            $"FeatureName:     {marcador.FeatureName}\n" +
            $"Locality:        {marcador.Locality}\n" +
            $"PostalCode:      {marcador.PostalCode}\n" +
            $"SubAdminArea:    {marcador.SubAdminArea}\n" +
            $"SubLocality:     {marcador.SubLocality}\n" +
            $"SubThoroughfare: {marcador.SubThoroughfare}\n" +
            $"Thoroughfare:    {marcador.Thoroughfare}\n";

            Console.WriteLine(direccion);
        }
    }
}
