using HealthMate.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HealthMate.ViewModels
{
    public enum MessageSender
    {
        Server,
        User
    };
    public class MessageDetailViewModel : BaseViewModel
    {
        private MeasuredItem item;
        private DateTime messageDateTime;
        private string message;
        private MessageSender sender;

    public DateTime MessageDateTime
        {
            get => messageDateTime;
            set => SetProperty(ref messageDateTime, value);
        }

        public MessageSender Sender
        {
            get => sender;
            set => SetProperty(ref sender, value);
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value); 
        }

        public MeasuredItem Item
        {
            get => item;
            set => SetProperty(ref item, value);
        }
    }
}
