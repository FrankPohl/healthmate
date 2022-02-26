using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using HealthMate.Models;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;

namespace HealthMate.Services
{
    public class RecognitionService
    {
        public event EventHandler<MeasuredItem> Recognized;


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
            Debug.WriteLine($"Evaluate Args {e.Result.IntentId} mit {e.Result.Text} und einigen Properties {e.Result.Properties.GetProperty("number")}");
            // we just checked whether something useful was said
            // if not we do nothing furhter processing of recognized text is done in the business logic
            if (!string.IsNullOrEmpty(e.Result.Text))
            {
                var item = new MeasuredItem();
                Recognized?.Invoke(this, item);
            }
        }
    }
}
