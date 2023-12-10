using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;

namespace FileEncrypter.Views
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        private Timer? _timer = null;
        private int seconds = 0;

        public LoadingWindow()
        {
            InitializeComponent();

            Closed += OnWindowClosed;
        }

        public void StartTimer()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += TimerTick;
            _timer.Start();
        }

        public void UpdateProgressBar(double value)
        {
            progressBar.Value = value;
            progressLabel.Content = $"{(int)value}%";
        }

        private void TimerTick(object? sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                seconds++;
                progressTimer.Content = $"Time elapsed: {seconds}";
            }));
        }

        private void OnWindowClosed(object? sender, EventArgs e)
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
    }
}
