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

        IntentRecognizer recognizer;
        private bool isListening;
        public async Task Init()
        {
            Debug.WriteLine("init start");
            SpeechConfig speechConfig = SpeechConfig.FromSubscription(Keys.LanguageUnderstandingSubscriptionKey, Keys.LanguageUnderstandingServiceRegion);
            speechConfig.SpeechRecognitionLanguage = "en-US";

            recognizer = new IntentRecognizer(speechConfig);
            LanguageUnderstandingModel intentModel = LanguageUnderstandingModel.FromAppId(Keys.LanguageUnderstandingAppId);
            recognizer.AddAllIntents(intentModel);
            recognizer.Recognized += Recognizer_Recognized;
            Debug.WriteLine("init end");
        }

        public async Task StartListening()
        {
            isListening = true;
            await recognizer.StartContinuousRecognitionAsync();
        }

        public async Task StopListening()
        {
            if (isListening)
            {
                await recognizer.StopContinuousRecognitionAsync();
                isListening = false;
            }
        }
        private void Recognizer_Recognized(object sender, IntentRecognitionEventArgs e)
        {
            Debug.WriteLine($"Evaluate Args mit Intent: {e.Result.IntentId} mit Text: {e.Result.Text} und einigen Entities: {e.Result.Entities.Count}");
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
