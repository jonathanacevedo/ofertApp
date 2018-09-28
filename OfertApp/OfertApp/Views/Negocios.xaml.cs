﻿using OfertApp.Models;
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
	public partial class Negocios : ContentPage
	{

        negocios viewModel;

        public Negocios ()
		{
			InitializeComponent();

            //BindingContext = viewModel = new ItemsViewModel();
            BindingContext = viewModel = new negocios();  //NegociosViewModel
        }


        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var negocio = args.SelectedItem as Negocio;
            if (negocio == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(negocio)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Negocios.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}