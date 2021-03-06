﻿using OfertApp.Models;
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
    public partial class OfertaUserDetailPage : ContentPage
    {
        ofertaDetailViewModel viewModel;
        private HttpClient cliente = new HttpClient();

        internal OfertaUserDetailPage(ofertaDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        public OfertaUserDetailPage()
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