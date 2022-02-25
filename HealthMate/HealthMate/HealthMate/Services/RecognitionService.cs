using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;

namespace HealthMate.Services
{
    public class RecognitionService
    {
        public async Task Init()
        {
            SpeechConfig speechConfig = SpeechConfig.FromSubscription(Keys.LanguageUnderstandingSubscriptionKey, Keys.LanguageUnderstandingServiceRegion);
            speechConfig.SpeechRecognitionLanguage = "en-US";

            var recognizer = new IntentRecognizer(speechConfig);
            LanguageUnderstandingModel intentModel = LanguageUnderstandingModel.FromAppId(Keys.LanguageUnderstandingAppId);
            recognizer.AddAllIntents(intentModel);
            recognizer.Recognized += Recognizer_Recognized;
            await recognizer.StartContinuousRecognitionAsync();
        }
        private void Recognizer_Recognized(object sender, IntentRecognitionEventArgs e)
        {
            Debug.WriteLine($"Evaluate Args {e.Result.IntentId} mit {e.Result.Text} und einigen Properties {e.Result.Properties.GetProperty("number")}" );
        }
    }
}
