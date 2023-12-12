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
using TaskManager.Models;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializaProcessFilter();
        }

        private void InitializaProcessFilter()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(processView.ItemsSource);
            view.Filter = ProcessFilter;
        }

        private bool ProcessFilter(object item)
        {
            ProcessModel process = (ProcessModel)item;

            return process.ProcessName.StartsWith(processFilterTextBox.Text);
        }

        private void ProcessFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // refresh filter
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(processView.ItemsSource);

            view.Refresh();
        }
    }
}
