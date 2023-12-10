using FileEncrypter.ViewModels;
using Microsoft.Win32;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileEncrypter.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new();
            DataContext = _viewModel;
        }

        private async void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.Encrypt();
        }

        private async void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.Decrypt();
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ///TextBox txtBox = (TextBox)sender;
                Keyboard.ClearFocus();
            }
        }

        private void FileDropArea_Drop(object sender, DragEventArgs e)
        {
            // retrive data
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                _viewModel.Source = files[0];
            }
        }

        private void OpenfileButton_Click(object sender, RoutedEventArgs e)
        {
            // show dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if(openFileDialog.ShowDialog() == true)
            {
                _viewModel.Source = openFileDialog.FileName;
            }
        }
    }
}
