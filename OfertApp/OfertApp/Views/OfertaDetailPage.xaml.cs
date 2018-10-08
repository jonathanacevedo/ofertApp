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
	public partial class OfertaDetailPage : ContentPage
	{
        ofertaDetailViewModel viewModel;

		internal OfertaDetailPage (ofertaDetailViewModel viewModel)
		{
			InitializeComponent ();

            BindingContext = this.viewModel = viewModel;
        }

        public OfertaDetailPage()
        {
            InitializeComponent();

            var oferta = new Oferta
            {
                producto = "Item 1",
                tipo = "This is an item description."
            };

            viewModel = new ofertaDetailViewModel(oferta);
            BindingContext = viewModel;
        }
    }
}