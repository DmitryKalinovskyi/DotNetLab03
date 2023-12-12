using EncryptLibrary;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using FileEncrypter.Views;

namespace FileEncrypter.ViewModels
{
    public class MainWindowViewModel: NotifyViewModel
    {
        public static string EncryptedExtension = ".crp";
        
        private IFileEncrypter _encrypter;

        public object CryptoKey
        {
            get
            {
                return _cryptoKey;
            }

            set
            {
                _cryptoKey = value;
                OnCryptoKeyChanged();
                OnPropertyChanged(nameof(CryptoKey));
            }
        }

        private object _cryptoKey;

        public string Source
        { 
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                OnSourceChanged();
                OnPropertyChanged(nameof(Source));
            }
        }

        private string _source;

        public bool EncryptButtonActive
        {
            get
            {
                return _encryptButtonActive;
            }
            set
            {
                _encryptButtonActive = value;
                OnPropertyChanged(nameof(EncryptButtonActive));
            }
        }

        private bool _encryptButtonActive;

        public bool DecryptButtonActive
        {
            get
            {
                return _decryptButtonActive;
            }
            set
            {
                _decryptButtonActive = value;
                OnPropertyChanged(nameof(DecryptButtonActive));
            }
        }

        private bool _decryptButtonActive;

        public MainWindowViewModel()
        {
            _encrypter = new EncryptLibrary.FileEncrypter(new HashBasedAlgorithm(0));
            _source = "";
            _cryptoKey = 0;
         //   _destination = "";
            DecryptButtonActive = false;
            EncryptButtonActive = false;
        }

        private void OnSourceChanged()
        {

            if(File.Exists(_source) == false)
            {
                _decryptButtonActive = false;
                _encryptButtonActive = false;
                return;
            }

            // get source file extension
            string ext = Path.GetExtension(_source);

            EncryptButtonActive = ext != EncryptedExtension;
            DecryptButtonActive = !_encryptButtonActive;
        }

        private void OnCryptoKeyChanged()
        {
            _encrypter.Algorithm = new HashBasedAlgorithm(_cryptoKey);
        }

        /// <summary>
        /// Computes destination by source file for encrypting
        /// </summary>
        /// <returns></returns>
        private string ComputeEncryptDestination()
        {
            // get absolute path without extension and add own extension

            string fileName = _source + EncryptedExtension;
            string directory = Directory.GetParent(_source).FullName;
            return Path.Combine(directory, fileName);
        }


        /// <summary>
        /// Compute desination by source file for decrypting
        /// </summary>
        /// <returns></returns>
        private string ComputeDecryptDestination()
        {
            string fileName = Path.GetFileNameWithoutExtension(_source);
            string directory = Directory.GetParent(_source).FullName;
            return Path.Combine(directory, fileName);
        }

        public async Task Encrypt()
        {
            try
            {
                // destination is a path where will be encrypted file, by default this is in the same directory with source file
                string destination = ComputeEncryptDestination();
               

                if(Path.Exists(destination))
                {
                    var result = MessageBox.Show($"There are already exist item with absolute name {destination}, do you want to continue Encryption? You can possibly lost your data",
                        "Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if(result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }

                // create new window with loading 
                LoadingWindow loadingWindow = new()
                {
                    Owner = App.Current.MainWindow
                };
                loadingWindow.Show();
                loadingWindow.StartTimer();

                Progress<double> progress = new(loadingWindow.UpdateProgressBar);

                var encryptResult = await _encrypter.EncryptAsync(Source, destination, progress);

                loadingWindow.Close();
                // close loading window, display info window


                // show file encryption info
                EncryptionInfoWindow encryptionInfoWindow = new()
                {
                    Owner = App.Current.MainWindow,
                    DataContext = encryptResult
                };
                encryptionInfoWindow.Show();

                //MessageBox.Show("File successfully encrypted!", "Operation completed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task Decrypt()
        {
            try
            {
                string destination = ComputeDecryptDestination();

                if (Path.Exists(destination)) 
                {
                    var result = MessageBox.Show($"There are already exist item with absolute name {destination}, do you want to continue Decryption? You can possibly lost your data",
                            "Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }

                // create new window with loading 
                LoadingWindow loadingWindow = new()
                {
                    Owner = App.Current.MainWindow
                };
                loadingWindow.Show();
                loadingWindow.StartTimer();
                Progress<double> progress = new(loadingWindow.UpdateProgressBar);


                var encryptResult = await _encrypter.DecryptAsync(Source, destination, progress);

                loadingWindow.Close();

                // show file encryption info
                EncryptionInfoWindow encryptionInfoWindow = new()
                {
                    Owner = App.Current.MainWindow,
                    DataContext = encryptResult
                };
                encryptionInfoWindow.Show();

                //MessageBox.Show("File successfully decrypted!", "Operation completed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
