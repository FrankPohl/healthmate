using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using HealthMate.Models;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;
using Newtonsoft.Json.Linq;

namespace HealthMate.Services
{
    public class RecognitionService
    {
        public event EventHandler<MeasuredItem> RecognizedIntent;
        public event EventHandler<String> RecognizedText;
        public event EventHandler<string> NothingProcessableRecognized;
        public event EventHandler CancelRecognized;
        public event EventHandler ConfirmRecognized;

        IntentRecognizer recognizer;
        private bool isListening;
        public async Task Init()
        {
            if (recognizer != null)
            {
                return;
            }
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
            // we just checked whether something useful was said
            // if not we do nothing furhter processing of recognized text is done in the business logic
            if (e.Result.Reason == ResultReason.RecognizedIntent)
            {
                RecognizedText?.Invoke(this, e.Result.Text);
                var item = new MeasuredItem();
                // for some reason the entities are empty but the json is filled. no time to evalate that during the hackathon
                // read all the entity info from the json to get detail info
                var json = JObject.Parse(e.Result.Properties.GetProperty(PropertyId.LanguageUnderstandingServiceResponse_JsonResult))["entities"];
                var entities = json.ToObject<List<Entity>>();
                json = JObject.Parse(e.Result.Properties.GetProperty(PropertyId.LanguageUnderstandingServiceResponse_JsonResult))["topScoringIntent"];
                var intentscore = json.ToObject<IntentScoring>();
                DateTime datePortion;
                TimeSpan timePortion;
                double dblValue = 0;
                int intValue;
                Debug.WriteLine($"Process intent {e.Result.IntentId} with score {intentscore.score}");
                if (intentscore.score < 0.3)
                {
                    NothingProcessableRecognized?.Invoke(this, e.Result.Text);
                    return;
                }
                // set the item structure with the estimated entities for the intents
                switch (e.Result.IntentId)
                {
                    case "BloodPressure":
                        item.MeasurementType = Measurement.BloodPressure;
                        intValue = GetFirstNumber(entities);
                        if (intValue > 0)
                        {
                            item.SysValue = intValue;
                        }
                        intValue = GetSecondNumber(entities);
                        if (intValue > 0)
                        {
                            item.DiaValue = intValue;
                        }
                        datePortion = GetDate(entities);
                        if (datePortion != DateTime.MinValue)
                            item.MeasurementDateTime = datePortion;
                        timePortion = GetTime(entities);
                        if (timePortion != TimeSpan.Zero)
                            item.MeasurementDateTime = item.MeasurementDateTime.Add(timePortion);
                        RecognizedIntent?.Invoke(this, item);
                        break;
                    case "Temperature":
                        item.MeasurementType = Measurement.Temperature;
                        dblValue = GetDoubleNumber(entities);
                        if (dblValue > 0)
                        {
                            item.MeasuredValue = dblValue;
                        }
                        datePortion = GetDate(entities);
                        if (datePortion != DateTime.MinValue)
                            item.MeasurementDateTime = datePortion;
                        timePortion = GetTime(entities);
                        if (timePortion != TimeSpan.Zero)
                            item.MeasurementDateTime = item.MeasurementDateTime.Add(timePortion);
                        RecognizedIntent?.Invoke(this, item);
                        break;
                    case "Pulse":
                        item.MeasurementType = Measurement.Pulse;
                        intValue = GetNumber(entities);
                        if (intValue > 0)
                        {
                            item.MeasuredValue = intValue;
                        }
                        datePortion = GetDate(entities);
                        if (datePortion != DateTime.MinValue)
                            item.MeasurementDateTime = datePortion;
                        timePortion = GetTime(entities);
                        if (timePortion != TimeSpan.Zero)
                            item.MeasurementDateTime = item.MeasurementDateTime.Add(timePortion);

                        RecognizedIntent?.Invoke(this, item);
                        break;
                    case "Glucose":
                        item.MeasurementType = Measurement.Glucose;
                        intValue = GetNumber(entities);
                        if (intValue > 0)
                        {
                            item.MeasuredValue = intValue;
                        }
                        datePortion = GetDate(entities);
                        if (datePortion != DateTime.MinValue)
                            item.MeasurementDateTime = datePortion;
                        timePortion = GetTime(entities);
                        if (timePortion != TimeSpan.Zero)
                            item.MeasurementDateTime = item.MeasurementDateTime.Add(timePortion);
                        RecognizedIntent?.Invoke(this, item);
                        break;
                    case "Utilities.Reject":
                    case "Utilities.Cancel":
                        CancelRecognized?.Invoke(this, EventArgs.Empty);
                        break;
                    case "Utilities.Confirm":
                        ConfirmRecognized?.Invoke(this, EventArgs.Empty);
                        break;
                    case "Calendar.CreateCalendarEntry":
                    case "DateTimeAck":
                        datePortion = GetDate(entities);
                        if (datePortion != DateTime.MinValue)
                            item.MeasurementDateTime = datePortion;
                        timePortion = GetTime(entities);
                        if (timePortion != TimeSpan.Zero)
                            item.MeasurementDateTime = item.MeasurementDateTime.Add(timePortion);
                        RecognizedIntent?.Invoke(this, item);
                        break;
                    default:
                        item.MeasurementType = Measurement.NotSet;
                        NothingProcessableRecognized?.Invoke(this, e.Result.Text);
                        break;
                }
            }
            else
            {
                NothingProcessableRecognized?.Invoke(this, e.Result.Text);
            }
        }
        private int GetNumber(List<Entity> result)
        {
            foreach (var entity in result)
            {
                if (entity.type == "builtin.number")
                {
                    Debug.WriteLine($"{entity.resolution.value}");
                    return Convert.ToInt16(entity.resolution.value);
                }
            }
            return 0;
        }
        private double GetDoubleNumber(List<Entity> result)
        {
            foreach (var entity in result)
            {
                if (entity.type == "builtin.number")
                {
                    return Convert.ToDouble(entity.resolution.value);
                }
            }
            return 0;
        }

        /// <summary>
        /// Find the second number for the sys/dia combination
        /// We expect the dia value to be given first
        /// </summary>
        /// <param name="result">List of all entities that are recognized in the utterance</param>
        /// <returns>The value of the diastole value or 0 if nothing is found</returns>
        private int GetSecondNumber(List<Entity> result)
        {
            int firstFoundPos = 0;
            foreach (var entity in result)
            {
                if (entity.type == "builtin.number")
                {
                    if (firstFoundPos == 0)
                    {
                        firstFoundPos = entity.startIndex;
                    }
                    else
                    {
                        if ((entity.startIndex > firstFoundPos) && (firstFoundPos > 0))
                        {
                            return Convert.ToInt16(entity.resolution.value);
                        }
                    }
                }
            }
            return 0;
        }
        /// <summary>
        /// Find the first number for the sys/dia combination
        /// We expect the sys value to be given first
        /// </summary>
        /// <param name="result">List of all entities that are recognized in the utterance</param>
        /// <returns>The value of the systole value or 0 if nothing is found. </returns>
        private int GetFirstNumber(List<Entity> result)
        {
            int firstFound = 0;
            int firstFoundPos = 0;
            foreach (var entity in result)
            {
                if (entity.type == "builtin.number")
                {
                    if (firstFoundPos == 0)
                    {
                        firstFoundPos = entity.startIndex;
                        firstFound = Convert.ToInt16(entity.resolution.value);
                    }
                    else
                    {
                        if (entity.startIndex < firstFoundPos)
                        {
                            firstFoundPos = entity.startIndex;
                            firstFound = Convert.ToInt16(entity.resolution.value);
                        }
                    }
                }
            }
            return firstFound;
        }
        private DateTime GetDate(List<Entity> result)
        {
            foreach (var entity in result)
            {
                if (entity.type == "builtin.datetimeV2.date" || entity.type == "builtin.datetimeV2.datetime" || entity.type == "Calendar.StartDate")
                {
                    string str;
                    if (entity.resolution?.values.Count > 0)
                    {
                        str = ((JObject)entity.resolution.values[0]).GetValue("value").ToString();
                    }
                    else
                    {
                        str = entity.resolution?.value;
                    }
                    if (!String.IsNullOrEmpty(str))
                    {
                        str = str.Substring(0, 10);
                        Debug.WriteLine($"Date Part: {str}");
                        return DateTime.Parse(str.Substring(0, 10));
                    }
                }
            }
            return DateTime.MinValue;
        }
        private TimeSpan GetTime(List<Entity> result)
        {
            foreach (var entity in result)
            {
                if (entity.type == "builtin.datetimeV2.date" || entity.type == "builtin.datetimeV2.datetime" || entity.type == "Calendar.StartTime")
                {
                    string str;
                    if (entity.resolution?.values.Count > 0)
                    {
                        str = ((JObject)entity.resolution.values[0]).GetValue("value").ToString();
                    }
                    else
                    {
                        str = entity.resolution?.value;
                    }
                    if ((!String.IsNullOrEmpty(str)) && (str?.Length > 11))
                    {
                        str = str.Substring(11);
                        Debug.WriteLine($"Time Part: {str}");
                        return TimeSpan.Parse(str);
                    }
                }
            }
            return TimeSpan.Zero;
        }
    }
}
