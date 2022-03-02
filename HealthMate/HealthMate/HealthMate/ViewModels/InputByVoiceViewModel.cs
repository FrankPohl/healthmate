using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HealthMate.ViewModels
{
    public class InputByVoiceViewModel : BaseViewModel
    {
        RecognitionService rc = new RecognitionService();

        private MeasuredItem _inputItem = new MeasuredItem() { MeasurementType = Measurement.NotSet, MeasurementDateTime = DateTime.MinValue };
        public ObservableCollection<MessageDetailViewModel> Messages { get; }

        private string WelcomeMessage = $"Hello. {Environment.NewLine}You can enter your heart rate, glucose or blood pressure by voice. {Environment.NewLine}For example, say  'My pulse was 73 at 9:30 am today.";
        public InputByVoiceViewModel()
        {
            Messages = new ObservableCollection<MessageDetailViewModel>();
            ToggleMicCommand = new Command(async () =>
            {
                if (IsListening)
                {
                    IsListening = false;
                    ProcessingState = "I'm muted. Tap on the mic icon to unmute.";
                    await rc.StopListening();
                    rc.RecognizedIntent -= Rc_Recognized;
                    rc.RecognizedText -= Rc_RecognizedText;
                    rc.ConfirmRecognized -= Rc_ConfirmRecognized;
                    rc.CancelRecognized -= Rc_CancelRecognized;
                    rc.NothingProcessableRecognized -= Rc_NothingProcessableRecognized;
                }
                else
                {
                    ProcessingState = "I'm listening";
                    IsListening = true;
                    rc.RecognizedIntent += Rc_Recognized;
                    rc.RecognizedText += Rc_RecognizedText;
                    rc.ConfirmRecognized += Rc_ConfirmRecognized;
                    rc.CancelRecognized += Rc_CancelRecognized;
                    rc.NothingProcessableRecognized += Rc_NothingProcessableRecognized;
                    await rc.StartListening();
                }
            });
        }

        private bool CheckNewItemComplete()
        {
            if (_inputItem.MeasurementType == Measurement.NotSet)
                return false;
            if (_inputItem.MeasurementDateTime == DateTime.MinValue)
                return false;

            if (_inputItem.MeasurementType == Measurement.BloodPressure)
            {
                if (_inputItem.SysValue == 0)
                    return false;
                if (_inputItem.DiaValue == 0)
                    return false;
            }
            else
            {
                if (_inputItem.MeasuredValue == 0)
                    return false;
            }

            // if we end up here every necessary field is filled
            return true;
        }

        // compare new info with already gathered info and ask for missing info
        private void EvaluateInoutAndIssueMessage(MeasuredItem item)
        {
            // Intent is the first thing we need
            Debug.WriteLine($"Intent: Old {_inputItem.MeasurementType} new {item.MeasurementType}");
            Debug.WriteLine($"Old Measurement: Old {_inputItem.MeasurementDateTime} new {item.MeasuredValue}");
            Debug.WriteLine($"Sys: Old {_inputItem.SysValue} new {item.SysValue}");
            Debug.WriteLine($"Dia: Old {_inputItem.DiaValue} new {item.DiaValue}");
            Debug.WriteLine($"Datum: Old {_inputItem.MeasurementDateTime} new {item.MeasurementDateTime}");
            if ((_inputItem.MeasurementType == Measurement.NotSet) && (item.MeasurementType == Measurement.NotSet))
            {
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = "You have to tell me what you want to enter heart rate, blood pressure or glucose" });
                return;
            }

            if (item.MeasurementType != Measurement.NotSet)
            {
                _inputItem.MeasurementType = item.MeasurementType;
            }

            if ((item.MeasuredValue > 0 ) && (_inputItem.MeasuredValue == 0))
            {
                Debug.WriteLine($"Set value to {item.MeasuredValue}");
                _inputItem.MeasuredValue = item.MeasuredValue;
            }

            if ((item.SysValue > 0) && (_inputItem.SysValue == 0))
            {
                Debug.WriteLine($"Set Sys value to {item.SysValue}");
                _inputItem.SysValue = item.SysValue;
            }
            if ((item.DiaValue > 0) && (_inputItem.DiaValue == 0))
            {
                Debug.WriteLine($"Set Dia value to {item.DiaValue}");
                _inputItem.DiaValue = item.DiaValue;
            }

            if ((item.MeasurementDateTime != DateTime.MinValue) && (_inputItem.MeasurementDateTime == DateTime.MinValue))
            {
                _inputItem.MeasurementDateTime = item.MeasurementDateTime;
            }

            if ((_inputItem.MeasuredValue== 0) && _inputItem.MeasurementType != Measurement.BloodPressure)
            {
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"What was the value of your measurment?" });
                return;
            }

            if (((_inputItem.SysValue == 0) || (_inputItem.DiaValue == 0)) && _inputItem.MeasurementType == Measurement.BloodPressure)
            {
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"Tell me the systolic and diastolic pressure values" });
                return;
            }

            if (_inputItem.MeasurementDateTime == DateTime.MinValue)
            {
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"When was the measurement taken?" });
                return;
            }


            if (CheckNewItemComplete())
            {
                // when we finally end up down here we have everything and ask for confirmation to save the data
                if (_inputItem.MeasurementType == Measurement.BloodPressure)
                {
                    Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"Do you want to save blood pressure {_inputItem.SysValue} to {_inputItem.DiaValue} measured on {_inputItem.MeasurementDateTime}?" });
                }
                else
                {
                    Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"Do you want to save value {_inputItem.MeasuredValue} for {_inputItem.MeasurementType} measured on {_inputItem.MeasurementDateTime}?" });
                }
            }
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            IsListening = true;
            rc.RecognizedIntent -= Rc_Recognized;
            rc.RecognizedText -= Rc_RecognizedText;
            rc.ConfirmRecognized -= Rc_ConfirmRecognized;
            rc.CancelRecognized -= Rc_CancelRecognized;
            rc.NothingProcessableRecognized -= Rc_NothingProcessableRecognized;
            await rc.Init();
            rc.RecognizedIntent += Rc_Recognized;
            rc.RecognizedText += Rc_RecognizedText;
            rc.ConfirmRecognized += Rc_ConfirmRecognized;
            rc.CancelRecognized += Rc_CancelRecognized;
            rc.NothingProcessableRecognized += Rc_NothingProcessableRecognized;
            await rc.StartListening();
            ProcessingState = "I'm listening.";
            Messages.Clear();
            Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"{WelcomeMessage}"});

            IsBusy = false;
        }

        private void Rc_CancelRecognized(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Messages.Clear();
                _inputItem = new MeasuredItem();
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"{WelcomeMessage}" });
            });
        }

        private void Rc_ConfirmRecognized(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (CheckNewItemComplete())
                {
                    await DataStore.AddItemAsync(_inputItem);
                    Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"I have saved your measurement. {Environment.NewLine}I'm waiting for the next measurement. Or you just close the app." });
                    _inputItem = new MeasuredItem();
                }

            });
        }


        private void Rc_NothingProcessableRecognized(object sender, string e)
        {
            if (!String.IsNullOrEmpty(e))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"I'm sorry. I did not know what you mean by {e}" });
                });
            }
        }

        private void Rc_RecognizedText(object sender, string e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.User, Message = e });
            });
        }

        private void Rc_Recognized(object sender, MeasuredItem e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Debug.WriteLine($"Something recognized {e.MeasuredValue} type {e.MeasurementType} date {e.MeasurementDateTime}");
                EvaluateInoutAndIssueMessage(e);
            });
        }

        public MeasuredItem InputItem
        {
            get => _inputItem;
            set
            {
                SetProperty(ref _inputItem, value);
            }
        }

        private string processingState;
        public string ProcessingState {
            get => processingState;
            private set
            {
                SetProperty(ref processingState, value);
            }
        }

        public ICommand ToggleMicCommand { get; private set; }

        private bool isListening;

        public bool IsListening
        {
            get => isListening;
            private set
            {
                SetProperty(ref isListening, value);
            }
        }
    }
}