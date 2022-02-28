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

        private MeasuredItem _inputItem = new MeasuredItem() { MeasurementType = Intent.None, MeasurementDateTime = DateTime.MinValue };
        public ObservableCollection<MessageDetailViewModel> Messages { get; }
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

        private void Rc_CancelRecognized(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Messages.Clear();
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"Hello. {Environment.NewLine} You can enter your heart rate, glucose or blood pressure by voice. {Environment.NewLine}For example, say  'My pulse was 73 at 9:30 am today." });
            });
        }

        private void Rc_ConfirmRecognized(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"Saving your data. {Environment.NewLine} Do you want to enter another measurment?" });
            });
        }

        // compare new info with already gathered info and ask for missing info
        private void EvaluateInoutAndIssueMessage(MeasuredItem item)
        {
            // Intent is the first thing we need
            Debug.WriteLine($"Alt Intent: {_inputItem.MeasurementType} neuer {item.MeasurementType}");
            Debug.WriteLine($"Alt Measurement: {_inputItem.Measurement} neuer {item.Measurement}");
            Debug.WriteLine($"Alt Sys: {_inputItem.SysValue} neuer {item.SysValue}");
            Debug.WriteLine($"Alt Dia: {_inputItem.DiaValue} neuer {item.DiaValue}");
            Debug.WriteLine($"Alt Dateum: {_inputItem.MeasurementDateTime} neuer {item.MeasurementDateTime}");
            if ((_inputItem.MeasurementType == Intent.None) && (item.MeasurementType == Intent.None))
            {
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = "You have to tell me what you want to enter heart rate, blood pressure or glucose" });
                return;
            }

            if (item.MeasurementType != Intent.None)
            {
                _inputItem.MeasurementType = item.MeasurementType;
            }

            if (item.Measurement > 0)
            {
                _inputItem.Measurement = item.Measurement;
            }

            if (item.SysValue > 0)
            {
                _inputItem.SysValue = item.SysValue;
            }
            if (item.DiaValue > 0)
            {
                _inputItem.DiaValue = item.DiaValue;
            }

            if (item.MeasurementDateTime != DateTime.MinValue)
            {
                _inputItem.MeasurementDateTime = item.MeasurementDateTime;
            }

            if ((_inputItem.Measurement == 0) && item.MeasurementType != Intent.BloodPressure)
            {
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"What was the value of your measurment?" });
                return;
            }


            if ((_inputItem.SysValue == 0) || (_inputItem.DiaValue == 0) && item.MeasurementType == Intent.BloodPressure)
            {
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"Tell me the systolic and diastolic pressure values" });
                return;
            }

            if (_inputItem.MeasurementDateTime == DateTime.MinValue)
            {
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"When was the measurement taken?" });
                return;
            }

            // when we finally end up down here we have everything and ask for confirmation to save the data
            if (_inputItem.MeasurementType == Intent.BloodPressure)
            {
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"Do you want to save blood pressure {_inputItem.SysValue} to {_inputItem.DiaValue} measured on {_inputItem.MeasurementDateTime}?" });
            }
            else
            {
                Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"Do you want to save value {_inputItem.Measurement} for {_inputItem.MeasurementType} measured on {_inputItem.MeasurementDateTime}?" });
            }
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            IsListening = true;
            await rc.Init();
            rc.RecognizedIntent += Rc_Recognized;
            rc.RecognizedText += Rc_RecognizedText;
            rc.ConfirmRecognized += Rc_ConfirmRecognized;
            rc.CancelRecognized += Rc_CancelRecognized;
            rc.NothingProcessableRecognized += Rc_NothingProcessableRecognized;
            await rc.StartListening();
            ProcessingState = "I'm listening.";
            Messages.Clear();
            Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"Hello. {Environment.NewLine} You can enter your pulse, glucose or blood pressure by voice. {Environment.NewLine}For example, say  'My pulse was 73 at 9:30 am today." });

            IsBusy = false;
        }

        private void Rc_NothingProcessableRecognized(object sender, string e)
        {
            if (!String.IsNullOrEmpty(e))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = $"I'm sorry. I cannot handle what you said. {e}." });
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
                Debug.WriteLine($"Something recognized {e.Measurement}");
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