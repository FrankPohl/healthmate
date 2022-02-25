using HealthMate.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMate.UWP.Services
{
    // from https://github.com/Azure-Samples/cognitive-services-speech-sdk/blob/master/samples/csharp/xamarin/kws-xamarin/kws-xamarin/kws-xamarin.UWP/Services/MicrophoneService.cs
    public class MicService : IMicService
    {
        public async Task<bool> GetPermissionsAsync()
        {
            bool isMicAvailable = true;
            try
            {
                var mediaCapture = new Windows.Media.Capture.MediaCapture();
                var settings = new Windows.Media.Capture.MediaCaptureInitializationSettings
                {
                    StreamingCaptureMode = Windows.Media.Capture.StreamingCaptureMode.Audio
                };
                await mediaCapture.InitializeAsync(settings);
            }
            catch (Exception)
            {
                isMicAvailable = false;
            }

            if (!isMicAvailable)
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-microphone"));
            }

            return isMicAvailable;
        }

        public void OnRequestPermissionsResult(bool isGranted)
        {
        }
    }
}
