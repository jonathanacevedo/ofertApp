using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OfertApp.Views;
using Xamarin.Forms;

//namespace OfertApp.Droid.Renderers
[assembly: ExportRenderer(typeof(LoginFacebookView),
                          typeof(OfertApp.Droid.Renderers.LoginFacebookRenderer))]

namespace OfertApp.Droid.Renderers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Android.App;
    using Models;
    using Newtonsoft.Json;
    using Xamarin.Auth;
    using Xamarin.Forms.Platform.Android;

    public class LoginFacebookRenderer : PageRenderer
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public LoginFacebookRenderer()
        {
            var activity = this.Context as Activity;

            var auth = new OAuth2Authenticator(
                clientId: "504256876713065",
                scope: "",
                authorizeUrl: new Uri(
                    "https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri(
                    "fb504256876713065://localhost/path"),
                isUsingNativeUI:true
                );

            auth.Completed += async (sender, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    var accessToken =
                        eventArgs.Account.Properties["access_token"].ToString();
                    var profile = await GetFacebookProfileAsync(accessToken);
                    Login.LoginFacebookSuccess(profile);
                }
                else
                {
                    Login.LoginFacebookFail();
                }
            };

            activity.StartActivity(auth.GetUI(activity));
        }
#pragma warning restore CS0618 // Type or member is obsolete

        async Task<FacebookResponse> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl = "https://graph.facebook.com/v2.8/me/?fields=" +
                "name,picture.width(999),cover,age_range,devices,email," +
                "gender,is_verified,birthday,languages,work,website," +
                "religion,location,locale,link,first_name,last_name," +
                "hometown&access_token=" + accessToken;
            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            var facebookResponse =
                JsonConvert.DeserializeObject<FacebookResponse>(userJson);
            return facebookResponse;
        }
    }
}




