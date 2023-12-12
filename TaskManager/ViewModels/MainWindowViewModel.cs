using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using TaskManager.Commands;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels
{
    public class MainWindowViewModel: NotifyViewModel
    {
        private static RelayCommand? _stopProcessCommand;

        public static RelayCommand StopProcessCommand
        {
            get
            {
                return _stopProcessCommand ??
                     (_stopProcessCommand = new RelayCommand(obj =>
                     {
                         try
                         {
                             ((ProcessModel)obj).StopProcess();   
                         }
                         catch (Exception ex)
                         {
                             MessageBox.Show("" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                         }
                     }));
            }
        }

        

        public ObservableCollection<ProcessModel> ProcessModels { get; set; }

        public IProcessWatcher ProcessWatcher { get; set; }

        public MainWindowViewModel()
        {
            ProcessModels = new ObservableCollection<ProcessModel>();
            ProcessWatcher = new ProcessWatcher();

            RetriveProcesses();
        }

        private async void RetriveProcesses()
        {
            try
            {
                var processes = await Task.Run(Process.GetProcesses);
                Trace.WriteLine("Processes Done!");

                foreach (var process in processes)
                {
                    if (process.ProcessName == "System") continue;
                    if (process.ProcessName == "Idle") continue;

                    AddProcess(process);
                }

                Trace.WriteLine("All processes added.");

                // start watcher
                ProcessWatcher.ProcessStarted += (s, e) => AddProcess(e.Process);
                ProcessWatcher.StartWatch();
                
            }
            catch(Win32Exception e)
            {
                MessageBox.Show(e.Message, "Acess denied. Please run application with administrator roots", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Process Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProcess(Process process)
        {
            var model = new ProcessModel(process);

            ProcessModels.Add(model);
            model.Exited += (s, e) => ProcessModels.Remove(model);
        }
    }
}
