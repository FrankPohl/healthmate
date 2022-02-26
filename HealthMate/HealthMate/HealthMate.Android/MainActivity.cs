using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using HealthMate.Services;
using HealthMate.Droid.Services;

namespace HealthMate.Droid
{
    [Activity(Label = "HealthMate", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private IMicService micService;
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Instance = this;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            Xamarin.Forms.DependencyService.Register<IMicService, MicService>();
            micService = Xamarin.Forms.DependencyService.Get<IMicService>();

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            
            switch (requestCode)
            { 
                //  1 is for record audio
                case 1:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            micService.OnRequestPermissionsResult(true);
                        }
                        else
                        {
                            micService.OnRequestPermissionsResult(false);
                        }
                    }
                    break;
            }
        }
    }
}