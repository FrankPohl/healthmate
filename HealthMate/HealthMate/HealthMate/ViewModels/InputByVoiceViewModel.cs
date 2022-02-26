using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HealthMate.ViewModels
{
    public class InputByVoiceViewModel : BaseViewModel
    {
        private MeasuredItem _inputItem = new MeasuredItem();

        public ObservableCollection<MessageDetailViewModel> Messages { get; }
        public InputByVoiceViewModel()
        {
            Messages = new ObservableCollection<MessageDetailViewModel>();
            LoadMockData();
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
            var rc = new RecognitionService();
            await rc.Init();
            rc.Recognized += Rc_Recognized;
            IsBusy = false;
        }

        private void Rc_Recognized(object sender, MeasuredItem e)
        {
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
    }
}