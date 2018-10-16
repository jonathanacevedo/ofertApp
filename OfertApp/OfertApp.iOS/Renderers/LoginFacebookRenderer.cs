using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using OfertApp.Views;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(LoginFacebookView),
                          typeof(OfertApp.iOS.Renderers.LoginFacebookRenderer))]

namespace OfertApp.iOS.Renderers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;
    using Xamarin.Auth;
    using Xamarin.Forms.Platform.iOS;

    public class LoginFacebookRenderer : PageRenderer
    {
        bool done = false;

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (done)
            {
                return;
            }

            var auth = new OAuth2Authenticator(
                clientId: "504256876713065",
                scope: "",
                authorizeUrl: new Uri(
                    "https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri(
                    "fb504256876713065://localhost/path"),
                isUsingNativeUI: true
                );
            auth.Completed += async (sender, eventArgs) =>
            {
                DismissViewController(true, null);
                Login.LoginFacebookFail();

                if (eventArgs.IsAuthenticated)
                {
                    var accessToken =
                        eventArgs.Account.Properties["access_token"].ToString();

                    var profile = await GetFacebookProfileAsync(accessToken);
                    Login.LoginFacebookSuccess(profile);
                }
                else
                {
                    Login.LoginFacebookSuccess(null);
                }
            };

            done = true;
            PresentViewController(auth.GetUI(), true, null);
        }


    }
}

