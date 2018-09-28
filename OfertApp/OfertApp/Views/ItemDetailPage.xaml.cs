using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using OfertApp.Models;
using OfertApp.ViewModels;

namespace OfertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

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