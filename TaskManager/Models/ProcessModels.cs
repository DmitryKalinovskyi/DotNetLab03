using System;
using System.Diagnostics;
using System.IO;
using TaskManager.Helpers;
using System.Windows.Media;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using TaskManager.Commands;
using System.Windows;

namespace TaskManager.Models
{
    // Each process model hide behind actual process data

    public class ProcessModel : NotifyModel
    {
        private RelayCommand? _changePriorityCommand;

        public RelayCommand ChangePriorityCommand
        {
            get
            {
                return _changePriorityCommand ??
                     (_changePriorityCommand = new RelayCommand(obj =>
                     {
                         try
                         {
                             // get new priority info and target process
                             ProcessPriorityClass priority = (ProcessPriorityClass)int.Parse((string)obj);

                             Priority = priority;
                         }
                         catch (Exception ex)
                         {
                             MessageBox.Show("" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                         }
                     }));
            }
        }

        public ImageSource Image => ImageHelper.GetImageSourceFromProcess(_process);

        public string ProcessName
        {
            get
            {
                try
                {
                    return _process.ProcessName;
                }
                catch
                {
                    return "?";
                }
            }
        }

        public long PID 
        {
            get
            {
                try
                {
                    return _process.Id;
                }
                catch
                {
                    return -1;
                }
            }
        }
        
        public string Status
        {
            get
            {
                try
                {
                    return _process.HasExited ? "Exited": "Running";
                }
                catch
                {
                    return "";
                }
            }
        }

        
        public int Threads
        {
            get
            {
                try
                {
                    return _process.Threads.Count;
                }
                catch { return -1; }
            }
        }

        public ProcessPriorityClass? Priority {
            get
            {
                try
                {
                    return _process.PriorityClass;
                }
                catch { return null; }
            }
            set
            {
                if (value == null) return;
                _process.PriorityClass = (ProcessPriorityClass)value;
                OnPropertyChanged(nameof(Priority));
            }
        }

        public long RAM {
            get
            {
                try
                {
                    return _process.WorkingSet64;
                }
                catch { return -1; }
            }
        }

        public DateTime? Started 
        {
            get
            {
                try
                {
                    return _process.StartTime;
                }
                catch { return null; }
            }
        }

        public Process Process => _process;

        public long RefreshCount => _refreshed;

        public EventHandler? Exited;

        

        private Process _process;

        private bool _terminated = false;
        private int _refreshed = 0;

        public ProcessModel(Process process)
        {
            _process = process ?? throw new ArgumentNullException(nameof(process));
            //_process.EnableRaisingEvents = true;
            _process.Exited += OnProcessExit;

            // invoke refreshing
            Task.Run(InvokeRefreshing);

        }

        public void StopProcess()
        {
            _process.Kill();
        }

        private async void InvokeRefreshing()
        {
            while (_terminated == false)
            {
                await Task.Delay(1000);

                Refresh();
            }
        }
        
        private void Refresh()
        {
            try
            {
                _refreshed++;
                _process.Refresh();
                UpdateAllProperties();
            }
            catch
            {
                Trace.WriteLine("Refresh error.");
            }
        }

        private void OnProcessExit(object? sender, EventArgs e)
        {
            _terminated = true; 
            Exited?.Invoke(this, e);
        }
    }
}
