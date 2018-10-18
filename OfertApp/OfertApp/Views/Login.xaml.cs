using Acr.UserDialogs;
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
        FacebookResponse respuestaPerfil;

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
            using (UserDialogs.Instance.Loading("Iniciando Sesión"))
            {
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Add("Accept", "application/json");
                String user = email.Text;
                String pass = password.Text;
                var uri = new Uri(String.Format(Constants.IP + ":8090/personas/loguear?usuario=" + user + "&password=" + pass, String.Empty));
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
        //email
        private async Task<FacebookResponse> GetFacebookProfileAsync()
        {
            String accessToken = "EAAJhtfiHJZBgBADGOcssYOcNAEFSpFsaL9o97diZCqSaM5XTPntiCAjofZABl4gAxMa04vvIaOZAdZBevcQmGo7EcZAv6Fza22FxNPN8iOXJxlsHogNqR3qKxZC3eqGhnrKlq1wnX8369Qdd0DkVP7yY49njEVLpxLYlPAeVCmMEwZDZD";
            var requestUrl = "https://graph.facebook.com/v2.8/me/?fields=" +
                "name,picture,cover,age_range,devices,email,gender," +
                "is_verified,birthday,work,website," +
                "location,locale,link,first_name,last_name," +
                "hometown&access_token=" + accessToken; // falla hacirndo esta solicitud
            var httpClient = new HttpClient();

            var userJson = await httpClient.GetStringAsync(requestUrl);
            FacebookResponse facebookResponse =
                JsonConvert.DeserializeObject<FacebookResponse>(userJson);

            return facebookResponse;
        }

        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            using (UserDialogs.Instance.Loading("Iniciando Sesión"))
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
                   "https://graph.facebook.com/me?fields=email";

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
                            respuestaPerfil = await GetFacebookProfileAsync();
                            //userFb = JsonConvert.DeserializeObject<FacebookResponse>(userJson);
                        }

                        if (account != null)
                        {
                            store.Delete(account, Constants.AppName);
                        }

                        await store.SaveAsync(account = e.Account, Constants.AppName);

                        if (accountLoggedIn.Equals("Google"))
                        {
                            RegistrarPersona(user.Email, user.Name);

                        }
                        else
                        {

                            String correo = respuestaPerfil.Email;
                            correo = correo.Replace(@"\\u0040", "@");
                            //await DisplayAlert("else de fb", correo, "ok");


                            RegistrarPersona(correo, respuestaPerfil.Name);
                        }


                        //await DisplayAlert("Email address", user.Email+", "+user.Name, "OK");
                    }
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
            App.Current.MainPage = new MainPage();
        } 
        #endregion


        //


        public async void RegistrarPersona(string email, string nombre)
        {
            cliente.DefaultRequestHeaders.Add("Accept", "application/json");

            Personas personas = new Personas();
            Persona person = new Persona();

            List<Persona> persona = new List<Persona>();
            
            if (accountLoggedIn.Equals("Google")){
                person.id = "67457";
                person.nombre = nombre;
                person.apellidos = "DEFAULT";
                person.correo = email;
                person.contrasena = "DEFAULT";
                person.telefono = "DEFAULT";
                person.genero = "DEFAULT";
                person.rol = "Cliente";
                person.estado = "Activo";
                person.token = "Google";
            }
            else
            {
                Console.WriteLine("ESTE ES EL PERFIL ");
                Console.WriteLine("PERFIL: " + respuestaPerfil.FirstName);
                person.nombre = respuestaPerfil.FirstName;    
                person.apellidos = respuestaPerfil.LastName;
                person.correo = respuestaPerfil.Email;
                person.contrasena = "DEFAULT";
                person.telefono = "DEFAULT";
                person.genero = "DEFAULT";
                person.rol = "DEFAULT";
                person.estado = "Activo";
                person.token = "Facebook";
            }

            persona.Add(person);
            personas.persona = persona;
            var json = JsonConvert.SerializeObject(personas);

            Console.WriteLine(json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await cliente.PostAsync(URL, content);

            if (response.IsSuccessStatusCode || response.StatusCode.Equals("202"))
            {
                var res = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(res);
                try
                {

                    var contenido = JsonConvert.SerializeObject(personas);
                    await SecureStorage.SetAsync("auth", contenido);

                   // await DisplayAlert("despues de setasync", "el facebook", "ok");

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error " + ex);
                }
                //await DisplayAlert("antes de llevar a mainpage", "el mainpage", "ok");

                Application.Current.MainPage = new MainPage();
                //await Navigation.PushAsync(new MenuPage());


                //App.Current.MainPage = new MainPage();
                //await Navigation.PushAsync(new MenuPage());
                //await DisplayAlert("despues de llevar a mainpage", "el mainpage", "ok");

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