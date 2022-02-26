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

        private MeasuredItem _inputItem = new MeasuredItem();
        private bool IsListening;
        public ObservableCollection<MessageDetailViewModel> Messages { get; }
        public InputByVoiceViewModel()
        {
            Messages = new ObservableCollection<MessageDetailViewModel>();
            LoadMockData();
            ToggleMicCommand = new Command(async () => 
            { 
                if (IsListening)
                {
                    IsListening = false;
                    ProcessingState = "Mute";
                    await rc.StopListening();
                    rc.Recognized -= Rc_Recognized;
                    

                }
                else
                {
                    ProcessingState = "Listening";
                    IsListening = true;
                    rc.Recognized += Rc_Recognized;
                    await rc.StartListening();
                }
            });
        }

        private void EvaluateInoutAndIssueMessage(MeasuredItem item)
        {
            
        }

        private void LoadMockData()
        {
            Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = "Servermessage" });
            Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.User, Message = "Usermessage" });
            Messages.Add(new MessageDetailViewModel() { Sender = MessageSender.Server, Message = "Ich brauch noch das datum" });
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            IsListening = true;
            await rc.Init();
            rc.Recognized += Rc_Recognized;
            await rc.StartListening();
            ProcessingState = "Listening";
            IsBusy = false;
        }

        private void Rc_Recognized(object sender, MeasuredItem e)
        {
            Debug.WriteLine($"Something recognized {e.Measurement}");
            EvaluateInoutAndIssueMessage(e);
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
    }
}