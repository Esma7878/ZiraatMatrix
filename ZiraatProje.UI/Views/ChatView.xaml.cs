using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ZiraatProje.UI.ViewModels;

namespace ZiraatProje.UI.Views
{
    public partial class ChatView : UserControl
    {
        private ChatViewModel? _attachedVm;

        public ChatView()
        {
            InitializeComponent();
            DataContextChanged += ChatView_DataContextChanged;
        }

        private void ChatView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_attachedVm != null)
            {
                _attachedVm.Messages.CollectionChanged -= Messages_CollectionChanged;
            }

            if (e.NewValue is ChatViewModel vm)
            {
                _attachedVm = vm;
                _attachedVm.Messages.CollectionChanged += Messages_CollectionChanged;
                ScrollToBottom();
            }
        }

        private void Messages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            Dispatcher.InvokeAsync(() =>
            {
                MessageScrollViewer?.ScrollToBottom();
            }, DispatcherPriority.Background);
        }
    }
}
