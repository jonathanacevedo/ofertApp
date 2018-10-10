using appOfertas.Models;
using Newtonsoft.Json;
using OfertApp.Models;
using OfertApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        
        Account account;
        AccountStore store;
        String accountLoggedIn;

        private const string URL = Constants.IP+":8050/orquestador/registrar/personas";
        private HttpClient cliente = new HttpClient();

        public Login ()
		{

            InitializeComponent();
            var facebook = botonFacebook;
            var google = botonGoogle;

            store = AccountStore.Create();
            facebook.GestureRecognizers.Add(new TapGestureRecognizer(loginFacebook));
            google.GestureRecognizers.Add(new TapGestureRecognizer(GoogleLoginClicked));

            account = store.FindAccountsForService(Constants.AppName).FirstOrDefault();
            
        }

        


        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistroPage());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");
            String user = email.Text;
            String pass = password.Text;
            var uri = new Uri(String.Format(Constants.IP+":8090/personas/loguear?usuario=" + user + "&password=" + pass, String.Empty));
            var response = await cliente.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Personas personas = JsonConvert.DeserializeObject<Personas>(content);
                Console.WriteLine(content);
                Console.WriteLine("Bienvenido " + personas.persona[0].nombre);
                try
                {
                    await SecureStorage.SetAsync("auth", content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error " + ex);
                }
                App.Current.MainPage = new MainPage();
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email o contraseña incorrectos", "OK");
                Console.WriteLine("Error");
            }
        }

        private void facebookClicked(View arg1, object arg2)
        {
            LoginWithFacebook();
        }


        async void LoginWithFacebook()
        {
            await App.Current.MainPage.Navigation.PushAsync(new LoginFacebookView());
        }

        private void GoogleLoginClicked(View arg1, object arg2)
        {
            string clientId = null;
            string redirectUri = null;
            accountLoggedIn = "Google";

            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.iOS:
                    clientId = Constants.iOSClientId;
                    redirectUri = Constants.iOSRedirectUrl;
                    break;

                case Xamarin.Forms.Device.Android:
                    clientId = Constants.AndroidClientId;
                    redirectUri = Constants.AndroidRedirectUrl;
                    break;
            }

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.Scope,
                new Uri(Constants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        private void loginFacebook(View arg1, object arg2)
        {
            OAuth2Authenticator auth = new OAuth2Authenticator(
                clientId: "504256876713065",
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("fb504256876713065://localhost/path"),
                // switch for new Native UI API
                //      true = Android Custom Tabs and/or iOS Safari View Controller
                //      false = Embedded Browsers used (Android WebView, iOS UIWebView)
                //  default = false  (not using NEW native UI)
                isUsingNativeUI: true
            );

            accountLoggedIn = "Facebook";

            auth.Completed += OnAuthCompleted;
            auth.Error += OnAuthError;

            AuthenticationState.Authenticator = auth;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(auth);

        }

        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            User user = null;
            if (e.IsAuthenticated)
            {
                String urlInfo = accountLoggedIn.Equals("Google") ? Constants.UserInfoUrl :
                "https://graph.facebook.com/me";

                // If the user is authenticated, request their basic user data from Google
                // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
                var request = new OAuth2Request("GET", new Uri(urlInfo), null, e.Account);



                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    FacebookResponse userFb = new FacebookResponse();
                    // Deserialize the data and store it in the account store
                    // The users email address will be used to identify data in SimpleDB
                    string userJson = await response.GetResponseTextAsync();
                    //user = JsonConvert.DeserializeObject<User>(userJson);

                    if (accountLoggedIn.Equals("Google"))
                    {
                        user = JsonConvert.DeserializeObject<User>(userJson);
                    }
                    else
                    {
                        userFb = JsonConvert.DeserializeObject<FacebookResponse>(userJson);
                    }

                    if (account != null)
                    {
                        store.Delete(account, Constants.AppName);
                    }

                    await store.SaveAsync(account = e.Account, Constants.AppName);

                    //RegistrarPersona(user.Email, user.Name);
                    //await DisplayAlert("Email address", user.Email+", "+user.Name, "OK");
                }
            }
        }
        //Login con Facebook

        #region Facebook
        public static Action LoginFacebookFail
        {
            get
            {
                return new Action(() => App.Current.MainPage =
                                  new NavigationPage(new Login()));
            }
        }

        public static void LoginFacebookSuccess(FacebookResponse profile)
        {
            App.Current.MainPage = new RegistroPage();
        } 
        #endregion


        //


        public async void RegistrarPersona(string email, string nombre)
        {
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");

            Personas personas = new Personas();
            Persona person = new Persona();

            List<Persona> persona = new List<Persona>();

            person.nombre = nombre;
            person.apellidos = "DEFAULT";
            person.correo = email;
            person.contrasena = "DEFAULT";
            person.telefono = "DEFAULT";
            person.genero = "DEFAULT";
            person.rol = "DEFAULT";
            person.estado = "Activo";
            person.token = "Google";

            persona.Add(person);

            personas.persona = persona;

            var json = JsonConvert.SerializeObject(personas);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await cliente.PostAsync(URL, content);
            
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                Console.WriteLine(res);
                try
                {
                    await SecureStorage.SetAsync("auth", nombre);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error " + ex);
                }
                App.Current.MainPage = new MainPage();
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email o contraseña incorrectos", "OK");
                Console.WriteLine("Error");
            }
            Console.WriteLine(personas.persona[0].nombre);

        }

        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            Console.WriteLine("Authentication error: " + e.Message);
        }
    }
}