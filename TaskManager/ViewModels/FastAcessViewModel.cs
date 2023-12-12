using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using TaskManager.Commands;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class FastAcessViewModel
    {
        private RelayCommand? _runApplication;

        public RelayCommand RunApplication
        {
            get
            {
                return _runApplication ??
                     (_runApplication= new RelayCommand(obj =>
                     {
                         try
                         {
                             string fullname = ((FastAcessModel)obj).Fullname;

                             ProcessStartInfo startInfo = new ProcessStartInfo
                             {
                                 FileName = fullname,
                                 UseShellExecute = true
                             };

                             Process.Start(startInfo);
                         }
                         catch (Exception ex)
                         {
                             MessageBox.Show("" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                         }
                     }));
            }
        }

        public ObservableCollection<FastAcessModel> FastAcess { get; set; }

        public FastAcessViewModel() 
        {
            FastAcess = new();
            InitializeFastAcess();
        }

        void InitializeFastAcess()
        {
            FastAcess.Add(new FastAcessModel("Calc", Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "\\system32\\calc.exe")));
            FastAcess.Add(new FastAcessModel("Paint", Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "\\system32\\mspaint.exe")));
            FastAcess.Add(new FastAcessModel("Command line", Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "\\system32\\cmd.exe")));
            FastAcess.Add(new FastAcessModel("Word", "C:\\Program Files\\Microsoft Office\\root\\Office16\\WINWORD.EXE"));
            FastAcess.Add(new FastAcessModel("Power shell", Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.System),
                "WindowsPowerShell\\v1.0\\powershell.exe")));
        }
    }
}
