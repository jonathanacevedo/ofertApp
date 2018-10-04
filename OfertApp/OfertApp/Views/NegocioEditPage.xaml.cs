using OfertApp.Models;
using OfertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NegocioEditPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public NegocioEditPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public NegocioEditPage()
        {
            InitializeComponent();
            var negocio = new Negocio
            {
                nombre = "negocio de Prueba",
                tipo = "Restaurante"
            };

            viewModel = new ItemDetailViewModel(negocio);
            BindingContext = viewModel;
        }
    }
}