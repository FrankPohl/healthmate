using HealthMate.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HealthMate.Helpers
{
    public class TemplateSelector:DataTemplateSelector
    {
        public DataTemplate ServerTemplate { get; set; }

        public DataTemplate UserTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var model = item as MessageDetailViewModel;

            switch (model.Sender)
            {
                case MessageSender.User:
                    return UserTemplate;
                case MessageSender.Server:
                default:
                    return ServerTemplate;
            }
        }
    }
}
