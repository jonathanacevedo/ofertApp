using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OfertApp.Views;
using Xamarin.Essentials;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace OfertApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Login());

            //MainPage = new MainPage();
        }

        protected async override void OnStart()
        {
            try
            {
                var oauthToken = await SecureStorage.GetAsync("auth");
                if (oauthToken != null)
                {
                    MainPage = new MainPage();
                }
                Console.WriteLine("Autenticacion: " + oauthToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
