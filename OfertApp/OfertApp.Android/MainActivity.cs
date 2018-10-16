using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms.Plus;
using Android.Util;
using Android.Graphics;
using System.Net;
using static Android.Resource;
using System.Threading.Tasks;
using Android;
using Acr.UserDialogs;

namespace OfertApp.Droid
{
    [Activity(Label = "OfertApp", Icon = "@drawable/imagenNegocio", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TryToGetPermissions();
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            UserDialogs.Init(this);



            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        protected override void OnStart()
        {
            base.OnStart();
        }


        public void OnConnected(Bundle connectionHint)
        {
            Console.WriteLine("Conectado");
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            throw new NotImplementedException();
        }

        public void OnConnectionSuspended(int cause)
        {
            throw new NotImplementedException();
        }

        #region RuntimePermissions

        private void TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                GetPermissions();
                return;
            }
        }
        const int RequestLocationId = 0;

        readonly string[] PermissionsGroupLocation =
            {
                            //TODO add more permissions
                            Manifest.Permission.AccessCoarseLocation,
                            Manifest.Permission.AccessFineLocation,
                            Manifest.Permission.ReadExternalStorage,
                            Manifest.Permission.WriteExternalStorage
             };
        void GetPermissions()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                //TODO change the message to show the permissions name
                Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();

                return;
            }

            if (ShouldShowRequestPermissionRationale(permission))
            {
                //set alert for executing the task

                    RequestPermissions(PermissionsGroupLocation, RequestLocationId);

                return;
            }
            RequestPermissions(PermissionsGroupLocation, RequestLocationId);

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                        {
                            //Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();
                        }
                        else
                        {
                            //Permission Denied :(
                            Toast.MakeText(this, "Permisos denegados", ToastLength.Short).Show();
                            TryToGetPermissions();
                        }
                    }
                    break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion
    }
}