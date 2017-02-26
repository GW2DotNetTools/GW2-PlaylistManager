using MahApps.Metro.Controls.Dialogs;
using System;

namespace PlaylistManager.Logic
{
    public class MessageBoxRaiseEvent : EventArgs
    {
        public MessageBoxRaiseEvent(string title, string message, MessageDialogStyle dialogStyle)
        {
            Title = title;
            Message = message;
            DialogStyle = dialogStyle;
        }

        public string Title { get; private set; }

        public string Message { get; private set; }

        public MessageDialogStyle DialogStyle { get; private set; }
    }
}
