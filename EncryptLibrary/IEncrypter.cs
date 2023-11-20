namespace EncryptLibrary
{
    public interface IEncrypter
    {
        public void Encrypt(string source, string destination);

        public void Decrypt(string source, string destination);
    }
}
