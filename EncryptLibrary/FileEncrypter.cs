namespace EncryptLibrary
{
    public class FileEncrypter : IFileEncrypter
    {
        public IEncryptingAlgorithm Algorithm { get; set; }

        public FileEncrypter() : this(new HashBasedAlgorithm(0)) { }

        public FileEncrypter(IEncryptingAlgorithm algorithm)
        {
            Algorithm = algorithm;
        }

        private FileEncryptionResult EncryptBase(string source, string destination, IProgress<double> progress, Func<byte, byte> encryptFunction)
        {

            if (File.Exists(source) == false)
                throw new FileNotFoundException($"File with path {source} not founded!");

            // to report progress
            FileInfo fileInfo = new FileInfo(source);
            long fileSize = fileInfo.Length;

            FileEncryptionResult fileEncryptionResult = new(fileSize, source, destination);

            using (FileStream fsInput = new(source, FileMode.Open))
            using (FileStream fsOutput = new(destination, FileMode.OpenOrCreate))
            {
                int b;
                int i = 0;
                int lastReported = -1;

                // Start timer
                fileEncryptionResult.EncryptionStart = DateTime.Now;

                while ((b = fsInput.ReadByte()) != -1)
                {
                    i++;

                    double value = (double)i / fileSize * 100;
                    if ((int)value > lastReported)
                    {
                        lastReported = (int)value;
                        progress.Report(value);
                    }

                    fsOutput.WriteByte(encryptFunction((byte)b));
                }
                
                // End timer
                fileEncryptionResult.EncryptionEnd = DateTime.Now;

                fileEncryptionResult.State = FileEncryptionResult.FileEncryptionState.Completed;
            }

            return fileEncryptionResult;
        }

        public FileEncryptionResult Encrypt(string source, string destination, IProgress<double> progress)
        {
            IEncryptingAlgorithm prototype = (IEncryptingAlgorithm)Algorithm.Clone();

            return EncryptBase(source, destination, progress, prototype.EncryptByte);   
        }

        public FileEncryptionResult Decrypt(string source, string destination, IProgress<double> progress)
        {
            IEncryptingAlgorithm prototype = (IEncryptingAlgorithm)Algorithm.Clone();

            return EncryptBase(source, destination, progress, prototype.DecryptByte);
        }

        public async Task<FileEncryptionResult> EncryptAsync(string source, string destination, IProgress<double> progress)
        {
            return await Task.Run(() => Encrypt(source, destination, progress));
        }

        public async Task<FileEncryptionResult> DecryptAsync(string source, string destination, IProgress<double> progress)
        {
            return await Task.Run(() => Decrypt(source, destination, progress));
        }
    }
}