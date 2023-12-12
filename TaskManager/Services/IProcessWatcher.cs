using System;
using System.Diagnostics;

namespace TaskManager.Services
{
    public class ProcessStartedEventArgs
    {
        public Process Process { get; set; }

        public ProcessStartedEventArgs(Process process)
        {
            Process = process;
        }
    }

    public interface IProcessWatcher
    {
        public event EventHandler<ProcessStartedEventArgs> ProcessStarted;

        public void StartWatch();

        public void EndWatch();
    }
}
