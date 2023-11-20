using System.Runtime.CompilerServices;

namespace EncryptLibrary
{
    public class Encrypter : IEncrypter
    {

        public string WorkingExtension { get; set; } = ".crp";

        /// <summary>
        /// Constant value for encryption algorithm
        /// </summary>
        private const long _pFactor = 31;

        /// <summary>
        /// Modulo used for encryption algorithm. Algorithm requires prime modulo, this is closeset for byte bound.
        /// </summary>
        private const long _modulo = 241;

        private long _cryptoKey = 0;

        public void UpdateCryptoKey(object obj)
        {
            _cryptoKey = obj.GetHashCode() % _modulo;
        }

        public void Encrypt(string source, string destination)
        {
            if (File.Exists(source) == false)
                throw new FileNotFoundException($"File with path {source} not founded!");

            using (FileStream fsInput = new(source, FileMode.Open))
            {
                using (FileStream fsOutput = new(destination, FileMode.OpenOrCreate))
                {
                    int b;

                    long step = 1;

                    while ((b = fsInput.ReadByte()) != -1)
                    {
                        byte setted = (byte)((b * step % _modulo) ^ _cryptoKey);
                        fsOutput.WriteByte(setted);

                        step = step * _pFactor % _modulo;
                    }
                }
            }
        }

        public void Decrypt(string source, string destination)
        {
            if (File.Exists(source) == false)
                throw new FileNotFoundException($"File with path {source} not founded!");

            using (FileStream fsInput = new(source, FileMode.Open))
            {
                using (FileStream fsOutput = new(destination, FileMode.OpenOrCreate))
                {
                    int b;
                    long inverse = 1;
                    long multiplier = MathExtension.ModuloInverse(_pFactor, _modulo);

                    while ((b = fsInput.ReadByte()) != -1)
                    {
                        byte updated = (byte)((b ^ _cryptoKey) * inverse % _modulo);

                        fsOutput.WriteByte(updated);

                        inverse = inverse * multiplier % _modulo;
                    }
                }
            }
        }
    }
}