using DevExpress.XtraEditors;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace Jankilla.Core.UI.WinForms.Listeners
{
    public class ListBoxTraceListener : TraceListener
    {
        private ListBoxControl _listBoxControl;
        private ConcurrentQueue<string> _messageQueue;
        private Timer _timer;

        public ListBoxTraceListener(ListBoxControl listBoxControl)
        {
            _listBoxControl = listBoxControl;
            _messageQueue = new ConcurrentQueue<string>();

            _timer = new Timer(ProcessQueue, null, 0, 100); 
        }

        public override void Write(string message)
        {
            _messageQueue.Enqueue(message);
        }

        public override void WriteLine(string message)
        {
            _messageQueue.Enqueue(message + Environment.NewLine);
        }

        private void ProcessQueue(object state)
        {
            while (_messageQueue.TryDequeue(out string message))
            {
                if (_listBoxControl.InvokeRequired)
                {
                    _listBoxControl.Invoke(new Action<string>(AppendMessage), message);
                }
                else
                {
                    AppendMessage(message);
                }
            }
        }

        private void AppendMessage(string message)
        {
            if (_listBoxControl.Items.Count > 1000)
            {
                _listBoxControl.Items.RemoveAt(0);
            }

            _listBoxControl.Items.Add(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
