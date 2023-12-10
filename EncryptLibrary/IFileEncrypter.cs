using System.Net;

namespace EncryptLibrary
{
    public interface IFileEncrypter
    {
        /// <summary>
        /// Defines the way of Encryption and Decryption.
        /// </summary>
        IEncryptingAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Make byte by byte file Encryption, from source to destination.
        /// </summary>
        /// <param name="source">Path to file that needed to encrypt</param>
        /// <param name="destination">Path to encrypted file</param>
        FileEncryptionResult Encrypt(string source, string destination, IProgress<double> progress);

        /// <summary>
        /// Make byte by byte file Decryption, from source to destination.
        /// </summary>
        /// <param name="source">Path to file that require decryption</param>
        /// <param name="destination">Path to decrypted file</param>
        FileEncryptionResult Decrypt(string source, string destination, IProgress<double> progress);

        Task<FileEncryptionResult> EncryptAsync(string source, string destination, IProgress<double> progress);

        Task<FileEncryptionResult> DecryptAsync(string source, string destination, IProgress<double> progress);

    }
}
