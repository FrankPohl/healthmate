using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Snackbar;
using HealthMate.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMate.Droid.Services
{
    internal class MicService:IMicService
    {
        public const int REQUEST_MIC = 1;
        private string[] permissions = { Manifest.Permission.RecordAudio, Manifest.Permission.ReadExternalStorage };
        private TaskCompletionSource<bool> tcsPermissions;

        public Task<bool> GetPermissionsAsync()
        {
            tcsPermissions = new TaskCompletionSource<bool>();

            // Permissions are required only for Marshmallow and up
            if ((int)Build.VERSION.SdkInt < 23)
            {
                tcsPermissions.TrySetResult(true);
            }
            else
            {
                var currentActivity = MainActivity.Instance;
                if ((ActivityCompat.CheckSelfPermission(currentActivity, Manifest.Permission.RecordAudio) != (int)Android.Content.PM.Permission.Granted) ||
                    ActivityCompat.CheckSelfPermission(currentActivity, Manifest.Permission.ReadExternalStorage) != (int)Android.Content.PM.Permission.Granted)
                {
                    RequestMicPermission();
                }
                else
                {
                    tcsPermissions.TrySetResult(true);
                }
            }
            return tcsPermissions.Task;
        }

        private void RequestMicPermission()
        {
            var currentActivity = MainActivity.Instance;
            if (ActivityCompat.ShouldShowRequestPermissionRationale(currentActivity, Manifest.Permission.RecordAudio))
            {
                Snackbar.Make(currentActivity.FindViewById((Android.Resource.Id.Content)), "App requires microphone permission.", Snackbar.LengthIndefinite).SetAction("Ok", v =>
                {
                    ((Activity)currentActivity).RequestPermissions(permissions, REQUEST_MIC);

                }).Show();
            }
            else
            {
                ActivityCompat.RequestPermissions(((Activity)currentActivity), permissions, REQUEST_MIC);
            }
        }

        public void OnRequestPermissionsResult(bool isGranted)
        {
            tcsPermissions.TrySetResult(isGranted);
        }
    }
}