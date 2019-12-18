using KawanApp.Models;
using KawanApp.Views.Cells;
using Xamarin.Forms;

namespace KawanApp.Helpers
{
    class ChatTemplateSelector : DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as ChatMessage;
            if (messageVm == null)
                return null;


            return (messageVm.SendingUser == App.CurrentUser) ? incomingDataTemplate : outgoingDataTemplate;
        }

    }
}