using System;
using System.Diagnostics;
using System.Management;

namespace TaskManager.Services
{
    public class ProcessWatcher : IProcessWatcher
    {
        public event EventHandler<ProcessStartedEventArgs>? ProcessStarted;

        private ManagementEventWatcher _watcher;
        private bool _watching = false;

        public ProcessWatcher() 
        {
            _watcher = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace")
                );
        }

        public void StartWatch()
        {
            _watching = true;

            _watcher.EventArrived += _watcher_EventArrived;

            _watcher.Start();
        }

        private void _watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            // get process and raise event
            
            int pid = Convert.ToInt32((uint)e.NewEvent.Properties["ProcessId"].Value);

            ProcessStarted?.Invoke(this, new ProcessStartedEventArgs(Process.GetProcessById(pid)));
        }

        public void EndWatch()
        {
            _watcher.Stop();
        }
    }
}
