namespace EncryptLibrary
{
    public interface IEncryptingAlgorithm: ICloneable
    {
        public byte EncryptByte(byte b);

        public byte DecryptByte(byte b);

        /// <summary>
        /// Clear cache collected during encryptinon and decryption
        /// </summary>
        public void ClearCache();
    }
}
