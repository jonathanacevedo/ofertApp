using OfertApp.Models;
using OfertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserDetailPage : ContentPage
	{

        ItemDetailViewModel viewModel;
        private HttpClient cliente = new HttpClient();

        public UserDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }


        public UserDetailPage ()
		{
			InitializeComponent ();
            var negocio = new Negocio
            {
                nombre = "Item 1",
                tipo = "This is an item description."
            };

            viewModel = new ItemDetailViewModel(negocio);
            BindingContext = viewModel;
        }
	}
}